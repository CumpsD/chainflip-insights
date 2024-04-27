namespace ChainflipInsights.Consumers.Database
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Json;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Feeders.StakedFlip;
    using CsvHelper;
    using Microsoft.Extensions.Logging;

    public partial class DatabaseConsumer
    {
        private void ProcessStakedFlipInfo(StakedFlipInfo stakedFlip)
        {
            if (!_configuration.DatabaseStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Database. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Staked FLIP in Database: {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .StakedInfo
                        .Add(new EntityFramework.StakedInfo
                        {
                            Date = stakedFlip.Date,
                            TotalMovement = stakedFlip.TotalMovement,
                            Staked = stakedFlip.Staked,
                            Unstaked = stakedFlip.Unstaked,
                            NetMovement = stakedFlip.NetMovement.ToString()
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Staked Flip {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                        stakedFlip.Date.ToString("yyyy-MM-dd"),
                        stakedFlip.StakedFormatted,
                        stakedFlip.UnstakedFormatted,
                        stakedFlip.TotalMovementFormatted,
                        stakedFlip.NetMovement);
                    
                    GenerateStakedFlipCsv(dbContext);
                }
                
                UploadStakedFlipCsv(File.ReadAllText(_configuration.StakedFlipCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateStakedFlipCsv(BotContext dbContext)
        {
            var stakedFlip = dbContext
                .StakedInfo
                .OrderBy(x => x.Date)
                .ToList();
            
            var funding = stakedFlip
                .Select(x => new
                {
                    StakedDate = x.Date.Date.ToString("yyyy-MM-dd"),
                    TotalMovement = x.TotalMovement,
                    Staked = x.Staked,
                    Unstaked = x.Unstaked,
                    NetMovement = x.NetMovement
                })
                .ToList();
     
            using var writer = new StreamWriter(_configuration.StakedFlipCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)funding.OrderBy(x => x.StakedDate).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Staked Flip CSV {CsvPath}",
                _configuration.StakedFlipCsvLocation);
        }
        
        private void UploadStakedFlipCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Staked Flip",
                    table_name = "flip_staked",
                };

                _logger.LogInformation(
                    "Uploading Staked Flip CSV {CsvPath}",
                    _configuration.StakedFlipCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Staked Flip CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Staked Flip CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Staked Flip CSV to Dune failed.");
            }
        }
    }
}