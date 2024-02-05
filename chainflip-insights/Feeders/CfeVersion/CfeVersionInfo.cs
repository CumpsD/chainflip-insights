namespace ChainflipInsights.Feeders.CfeVersion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Semver;

    public class CfeVersionsInfo
    {
        public string Date { get; }
        
        public Dictionary<SemVersion, CfeVersionInfo> Versions { get; }
        
        public CfeVersionsInfo(
            string date, 
            IEnumerable<CfeVersionInfo> cfeVersions)
        {
            Date = date;
            Versions = cfeVersions.ToDictionary(
                x => SemVersion.Parse(x.Version, SemVersionStyles.Strict),
                x => x);
        }
    }
    
    public class CfeVersionInfo
    {
        public string Version { get; }
        
        public List<CfeVersionValidatorInfo> Validators { get; }
        
        public CfeVersionInfo(CfeVersionInfoResponseNode cfeVersion, double lastBlockId)
        {
            Version = cfeVersion.Id;

            Validators = cfeVersion
                .Validators
                .Data
                .Select(x => new CfeVersionValidatorInfo(x.Data.Name, x.Data.LastHeartBeat, lastBlockId))
                .ToList();
        }
    }

    public enum ValidatorStatus
    {
        Online,
        Offline
    }
    
    public class CfeVersionValidatorInfo
    {
        private const int OfflineBlockCount = 150 * 2; // Taking twice as a margin
        
        public string Name { get; }

        public double? LastHeartBeat { get; }
        
        public ValidatorStatus ValidatorStatus { get; }

        public CfeVersionValidatorInfo(
            string name,
            double? lastHeartBeat, 
            double lastBlockId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LastHeartBeat = lastHeartBeat;

            if (lastHeartBeat == null)
            {
                ValidatorStatus = ValidatorStatus.Offline;
            } 
            else if (lastHeartBeat < lastBlockId - OfflineBlockCount)
            {
                ValidatorStatus = ValidatorStatus.Offline;
            }
            else
            {
                ValidatorStatus = ValidatorStatus.Online;
            }
        }
    }
}