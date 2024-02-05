namespace ChainflipInsights.Feeders.CfeVersion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CfeVersionsInfo
    {
        public CfeVersionInfo[] Versions { get; }
        
        public CfeVersionsInfo(
            CfeVersionInfo[] cfeVersions) 
            => Versions = cfeVersions;
    }
    
    public class CfeVersionInfo
    {
        public string Version { get; }
        
        public List<CfeVersionValidatorInfo> Validators { get; }
        
        public CfeVersionInfo(
            CfeVersionInfoResponseNode cfeVersion)
        {
            Version = cfeVersion.Id;

            Validators = cfeVersion
                .Validators
                .Data
                .Select(x => new CfeVersionValidatorInfo(x.Data.Name, x.Data.LastHeartBeat))
                .ToList();
        }
    }

    public class CfeVersionValidatorInfo
    {
        public string Name { get; }

        public double? LastHeartBeat { get; }

        public CfeVersionValidatorInfo(
            string name,
            double? lastHeartBeat)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LastHeartBeat = lastHeartBeat;
        }
    }
}