using Mayhem.Configuration.Interfaces;
using Mayhem.Explore.Mission.Worker.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Mayhem.Explore.Mission.Worker
{
    public class ExploreMissionBackgroundWorker : IHostedService
    {
        private readonly System.Timers.Timer timer;
        private readonly IExploreMissionService exploreMissionService;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public ExploreMissionBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            IExploreMissionService exploreMissionService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.exploreMissionService = exploreMissionService;
            timer = new System.Timers.Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.RunExploreMissionWorkerTimeInSeconds).TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        protected async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await exploreMissionService.StartWorkAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}