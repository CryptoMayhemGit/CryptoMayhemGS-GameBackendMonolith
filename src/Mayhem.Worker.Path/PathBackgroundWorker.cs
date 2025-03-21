using Mayhem.Configuration.Interfaces;
using Mayhem.Worker.Path.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Mayhem.Worker.Path
{
    public class PathBackgroundWorker : IHostedService
    {
        private readonly System.Timers.Timer timer;
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IPathWorkerService pathWorkerService;

        public PathBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            IPathWorkerService pathWorkerService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.pathWorkerService = pathWorkerService;
            timer = new System.Timers.Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.RunPathWorkerInSeconds).TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        protected async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await pathWorkerService.StartWorkAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
