using Mayhem.Dal.Dto.Dtos;
using Mayhem.Land.Bl.Dtos;
using Mayhem.Land.Bl.Interfaces;
using Mayhem.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Land.Worker
{
    public class LandGeneratorBackgroundService : BackgroundService
    {
        private readonly ILogger<LandGeneratorBackgroundService> logger;
        private readonly ILandGeneratorService landGeneratorService;

        public LandGeneratorBackgroundService(
            ILogger<LandGeneratorBackgroundService> logger,
            ILandGeneratorService landGeneratorService)
        {
            this.logger = logger;
            this.landGeneratorService = landGeneratorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                List<ImportLandDto> lands = await landGeneratorService.ReadLandsAsync();
                IEnumerable<LandDto> landsDb = await landGeneratorService.SaveLandsAsync(lands);
                stopwatch.Stop();
                if (landsDb.Any())
                {
                    logger.LogInformation(LoggerMessages.SuccessfullyGeneratedPackages(lands.Count, stopwatch.ElapsedMilliseconds));
                }
                else
                {
                    logger.LogCritical(LoggerMessages.GeneratingLandsFailed);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggerMessages.ErrorOccurredDuring("generate lands"));
            }
        }
    }
}
