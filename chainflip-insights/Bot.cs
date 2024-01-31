//             var swapStartedAt = DateTimeOffset.FromUnixTimeMilliseconds(swapInfo.DepositReceivedAt);
//             var swapFinishedAt = DateTimeOffset.FromUnixTimeMilliseconds(swapInfo.BroadcastSucceededAt);
//             var swapTime = swapFinishedAt.Subtract(swapStartedAt);
//
//             var time = DateTimeOffset.Parse(swap.SwapScheduledBlockTimestamp);
//
//             _logger.LogInformation(
//                 "Swap {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} at {SwapTime} -> {ExplorerUrl}",
//                 Math.Round(swapInput, 8).ToString(inputString),
//                 swap.SourceAsset,
//                 Math.Round(swapOutput, 8).ToString(outputString),
//                 swap.DestinationAsset,
//                 $"{time:yyyy-MM-dd HH:mm:ss}",
//                 $"{_configuration.ExplorerUrl}{swap.Id}");
//
//         private static string HumanTime(TimeSpan span)
//         {
//             var time = new StringBuilder();
//
//             if (span.Hours > 0)
//                 time.Append($"{span.Hours}h");
//             
//             if (span.Minutes > 0)
//                 time.Append($"{span.Minutes}m");
//
//             if (span.Seconds > 0)
//                 time.Append($"{span.Seconds}s");
//
//             return time.ToString();
//         }
