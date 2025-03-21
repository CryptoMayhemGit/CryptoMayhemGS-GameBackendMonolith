using Mayhem.Configuration.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Mayhem.Worker.TransferListener.Base
{
    public abstract class BaseBackgroundWorker : IHostedService
    {
        private readonly System.Timers.Timer timer;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        protected BaseBackgroundWorker(IMayhemConfigurationService mayhemConfigurationService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            timer = new System.Timers.Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.TransferIntervalInSeconds).TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        protected abstract void Timer_Elapsed(object sender, ElapsedEventArgs e);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
