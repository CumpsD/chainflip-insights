namespace ChainflipInsights.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Security.Cryptography;

    public static class AddressValidator
    {
        // Base58 prefixes
        private static readonly int[] BitcoinBase58MainnetPrefixes = { 0, 5 };
        private static readonly int[] BitcoinBase58TestnetPrefixes = { 111, 196 };
        private static readonly int[] LitecoinBase58MainnetPrefixes = { 5, 48, 50 };
        private static readonly int[] LitecoinBase58TestnetPrefixes = { 111, 196, 58 };

        // Bech32 human readable parts
        private const string BitcoinBech32MainnetHrp = "bc1";

        private const string BitcoinBech32TestnetHrp = "tb1";

        // private const string BitcoinBech32RegnetHrp = "bcrt1";
        private const string LitecoinBech32MainnetHrp = "ltc1";
        private const string LitecoinBech32TestnetHrp = "tltc1";
        // private const string LitecoinBech32RegnetHrp = "rtlc1";

        /*
         * <summary>
         * Checks if a given cryptocurrency address fulfills offline validity criteria (such as encoding, checksum,
         * and magic numbers).
         * </summary>
         *
         * <param name="address">address in Bech32 or Base58 encoding</param>
         * <param name="currency">currency of the address, e.g. btc or ltc (case-insensitive)</param>
         * <param name="testnet">(optional) set to true in order to validate Testnet addresses as opposed to Mainnet addresses</param>
         *
         * <returns>true iff address was successfully validated</returns>
         *
         * <exception cref="FormatException">thrown if something was wrong with the encoding or checksum</exception>
         * <exception cref="ArgumentException">thrown if the currency could not be recognized or is not supported</exception>
         * <exception cref="ArgumentNullException">thrown if an argument was null</exception>
         */
        public static bool IsValidAddress(string address, string currency, bool testnet = false)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (currency == null) throw new ArgumentNullException(nameof(address));
            switch (currency.ToLower())
            {
                case "btc":
                    if (address.ToLower().StartsWith(!testnet ? BitcoinBech32MainnetHrp : BitcoinBech32TestnetHrp))
                        return ValidateBech32Address(address);
                    return ValidateBase58Address(address,
                        !testnet ? BitcoinBase58MainnetPrefixes : BitcoinBase58TestnetPrefixes);
                case "ltc":
                    if (address.ToLower().StartsWith(!testnet ? LitecoinBech32MainnetHrp : LitecoinBech32TestnetHrp))
                        return ValidateBech32Address(address);
                    return ValidateBase58Address(address,
                        !testnet ? LitecoinBase58MainnetPrefixes : LitecoinBase58TestnetPrefixes);
                default:
                    throw new ArgumentException("currency not recognized");
            }
        }

        private static bool ValidateBech32Address(string address)
        {
            return Bech32.Decode(address) != null;
        }

        private static bool ValidateBase58Address(string address, int[] prefixes)
        {
            return Base58.Decode(address, prefixes) != null;
        }

        /*
         * <summary>
         * Helper class to work with Bitcoin's Bech32 encoded address strings. Fully implements BIP-173 decoder specification.
         * </summary>
         */
        private static class Bech32
        {
            private static readonly uint[] PolymodGenerator =
                { 0x3b6a57b2U, 0x26508e6dU, 0x1ea119faU, 0x3d4233ddU, 0x2a1462b3U };

            /*
             * <summary>
             * Converts a Bech32 encoded string to a byte array, validates the checksum and some segwit properties.
             * </summary>
             *
             * <param name="address">a Bech32 encoded wallet address</param>
             *
             * <returns>segwit address data part (i.e. without human readable part and separator) as byte array</returns>
             *
             * <exception cref="FormatException">thrown if something was wrong with the encoding or checksum</exception>
             */
            internal static byte[] Decode(string address)
            {
                if (address.Any(char.IsUpper) && address.Any(char.IsLower))
                    throw new FormatException(
                        "Error at Bech32 decoding, only all lowercase and all uppercase Bech32 addresses allowed");
                var base32Array = ToBase32ArrayCheckAndStripChecksum(address.ToLower());
                return DecodeSegwitDataPart(base32Array);
            }

            private static byte[] DecodeSegwitDataPart(byte[] base32Array)
            {
                if (base32Array[0] > 16) throw new FormatException("Error at Bech32 decoding, invalid witness version");
                var bitArraySource = new BitArray(base32Array.SubArray(1, base32Array.Length - 1));
                var destinationBitCountWithZeroBits =
                    (bitArraySource.Length / 8 + (bitArraySource.Length % 8 == 0 ? 0 : 1)) * 5;
                var destinationBitCountWithOutZeroBits = destinationBitCountWithZeroBits / 8 * 8;
                if (destinationBitCountWithOutZeroBits - destinationBitCountWithZeroBits > 4)
                    throw new FormatException(
                        "Error at Bech32 decoding, incomplete last witness program byte has more than 4 bit");
                for (var i = 0; i < destinationBitCountWithOutZeroBits - destinationBitCountWithZeroBits; i++)
                {
                    if (bitArraySource[bitArraySource.Length - i])
                        throw new FormatException(
                            "Error at Bech32 decoding, incomplete last witness program byte not zero");
                }

                var bitArrayDestination = new BitArray(destinationBitCountWithOutZeroBits);
                for (int i = 0, j = 0; i < bitArraySource.Length && j < destinationBitCountWithOutZeroBits; i++)
                {
                    if (i % 8 < 3) continue;
                    bitArrayDestination[j] = bitArraySource[i];
                    j++;
                }

                var byteArray = new byte[bitArrayDestination.Length / 8];
                bitArrayDestination.CopyTo(byteArray, 0);
                if (byteArray.Length < 2 || byteArray.Length > 40)
                    throw new FormatException(
                        "Error at Bech32 decoding, witness program not between 2 and 40 bytes long");
                if (base32Array[0] == 0 && byteArray.Length != 20 && byteArray.Length != 32)
                    throw new FormatException("Error at Bech32 decoding, version byte is zero but witness " +
                                              "program neither 20 nor 32 bytes long and therefore violates BIP141");
                return byteArray.Prepend(base32Array[0]).ToArray();
            }

            private static byte[] ToBase32ArrayCheckAndStripChecksum(string inputBech32)
            {
                var expandedHrp = ExpandHrp(inputBech32.Substring(0, inputBech32.LastIndexOf('1')));
                var data = ToBase32Array(inputBech32.Substring(inputBech32.LastIndexOf('1') + 1,
                    inputBech32.Length - (inputBech32.LastIndexOf('1') + 1)));
                return Polymod(expandedHrp.Concat(data)) != 1
                    ? throw new FormatException("Error at Bech32 decoding, invalid checksum")
                    : data.SubArray(0, data.Length - 6);
            }

            private static uint Polymod(IEnumerable<byte> inputArray)
            {
                uint chk = 1;
                foreach (var value in inputArray)
                {
                    var top = chk >> 25;
                    chk = (value ^ ((chk & 0x1ffffff) << 5));
                    chk = Enumerable
                        .Range(0, 5)
                        .Aggregate(chk, (current, i) => current ^ (((top >> i) & 1) == 1 ? PolymodGenerator[i] : 0));
                }

                return chk;
            }

            private static byte[] ToBase32Array(string inputBase32)
            {
                var outputArray = new byte[inputBase32.Length];
                const string alphabet = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

                for (var i = 0; i < inputBase32.Length; i++)
                {
                    if (alphabet.Contains(inputBase32[i]))
                        outputArray[i] = (byte)alphabet.IndexOf(inputBase32[i]);
                    else
                        throw new FormatException("Error at Bech32 decoding, invalid character in Bech32 address: " +
                                                  inputBase32[i]);
                }

                return outputArray;
            }

            private static IEnumerable<byte> ExpandHrp(string inputHrp)
            {
                var hrpLength = inputHrp.Length;
                var expandedHrp = new byte[2 * hrpLength + 1];
                for (var i = 0; i < hrpLength; i++)
                {
                    expandedHrp[i] = (byte)(inputHrp[i] >> 5);
                    expandedHrp[i + hrpLength + 1] = (byte)(inputHrp[i] & 31);
                }

                return expandedHrp;
            }
        }

        /*
         * <summary>
         * Helper class to work with Bitcoin's base58 encoded address strings
         * </summary>
         */

        private static class Base58
        {
            /*
             * <summary>
             * Converts a Base58 string to a byte array, validates the checksum and checks the prefix .
             * </summary>
             *
             * <param name="address">a Base58 encoded wallet address</param>
             * <param name="prefixes">the protocol and network dependent address prefixes to check for in the encoding</param>
             *
             * <returns>address as byte array</returns>
             *
             * <exception cref="FormatException">thrown if something was wrong with the encoding or checksum</exception>
             */
            internal static byte[] Decode(string address, int[] prefixes)
            {
                if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
                if (prefixes == null) throw new ArgumentNullException(nameof(prefixes));
                if (prefixes.Length == 0)
                    throw new ArgumentException("Error at Base58 decoding, at least one prefix is required",
                        nameof(prefixes));
                var decoded = ToByteArrayCheckAndStripChecksum(address);
                if (decoded == null || decoded.Length != 21)
                    throw new FormatException("Error at Base58 decoding, " +
                                              "address too short or could not be decoded");
                if (!prefixes.Contains(decoded[0])) throw new FormatException("Error at Base58 decoding, wrong prefix");
                return decoded;
            }

            private static byte[] ToByteArrayCheckAndStripChecksum(string inputBase58)
            {
                // convert base58 encoding to byte array
                var inputArray = ToByteArray(inputBase58);
                if (inputArray == null || inputArray.Length < 4)
                    throw new FormatException("Error at Base58 decoding, encoding too short");

                // compute and check checksum
                var hasher = new SHA256Managed();
                var hash = hasher.ComputeHash(inputArray.SubArray(0, inputArray.Length - 4));
                hash = hasher.ComputeHash(hash);
                if (!inputArray.SubArray(21, 4).SequenceEqual(hash.SubArray(0, 4)))
                    throw new FormatException("Error at Base58 decoding, bad checksum");

                // strip checksum and return
                return inputArray.SubArray(0, inputArray.Length - 4);
            }

            private static byte[] ToByteArray(string inputBase58)
            {
                var outputValue = new BigInteger(0);
                const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
                foreach (var character in inputBase58)
                {
                    if (alphabet.Contains(character))
                        outputValue = BigInteger.Add(new BigInteger(alphabet.IndexOf(character)),
                            BigInteger.Multiply(outputValue, new BigInteger(58)));
                    else
                        throw new FormatException("Error at Base58 decoding, invalid character in base58 address: " +
                                                  character);
                }

                var outputArray = outputValue.ToByteArray(true, true);
                // interpret leading 1s as leading zero bytes as per specification
                foreach (var character in inputBase58)
                {
                    if (character != '1') break;
                    var extendedArray = new byte[outputArray.Length + 1];
                    Array.Copy(outputArray, 0, extendedArray, 1, outputArray.Length);
                    outputArray = extendedArray;
                }

                return outputArray;
            }
        }
    }

    /*
     * Helper class to generate sub array
     */
    internal static class ArrayExtensions
    {
        /*
         * <summary>
         * Generates sub array with subset of consecutive elements
         * </summary>
         *
         * <param name="index">index of the original array to start the sub array at</param>
         * <param name="length">length of the desired sub array</param>
         *
         * <returns>sub array from original array starting at index and with length</returns>
         */
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}