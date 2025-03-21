using Mayhem.Configuration.Interfaces;
using Mayhem.Worker.Notification.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Mayhem.Worker.Notification
{
    public class NotificationBackgroundWorker : IHostedService
    {
        private readonly System.Timers.Timer timer;
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IWorkerService workerService;

        public NotificationBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            IWorkerService workerService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.workerService = workerService;
            timer = new System.Timers.Timer();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromMinutes(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.TryResendNotificationTimeInMinutes).TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        protected async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await workerService.ResendNotificationsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
