using Mayhem.Configuration.Interfaces;
using Mayhem.Discovery.Mission.Worker.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Mayhem.Discovery.Mission.Worker
{
    public class DiscoveryMissionBackgroundWorker : IHostedService
    {
        private readonly System.Timers.Timer timer;
        private readonly IDiscoveryMissionService discoveryMissionService;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public DiscoveryMissionBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            IDiscoveryMissionService discoveryMissionService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.discoveryMissionService = discoveryMissionService;
            timer = new System.Timers.Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.RunDiscoveryMissionWorkerTimeInSeconds).TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        protected async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await discoveryMissionService.StartWorkAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}