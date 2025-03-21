using Mayhem.Configuration.Interfaces;
using Mayhem.Messages;
using Mayhem.Package.Bl.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Package.Worker
{
    public class PackageGeneratorBackgroundService : BackgroundService
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IPackageService packageService;
        private readonly ILogger<PackageGeneratorBackgroundService> logger;

        public PackageGeneratorBackgroundService(
            IPackageService packageService,
            IMayhemConfigurationService mayhemConfigurationService,
            ILogger<PackageGeneratorBackgroundService> logger)
        {
            this.packageService = packageService;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                IEnumerable<Dal.Dto.Classes.Generator.Package> packages = packageService.GenerateAndValidate(mayhemConfigurationService.MayhemConfiguration.GeneratorConfiguration.PackageAmount);
                bool result = await packageService.InsertNftsAsync(packages);
                stopwatch.Stop();
                if (result)
                {
                    logger.LogInformation(LoggerMessages.SuccessfullyGeneratedPackages(packages.Count(), stopwatch.ElapsedMilliseconds));
                }
                else
                {
                    logger.LogCritical(LoggerMessages.GeneratingPackagesFailed);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggerMessages.ErrorOccurredDuring("generate packages"));
            }
        }
    }
}
