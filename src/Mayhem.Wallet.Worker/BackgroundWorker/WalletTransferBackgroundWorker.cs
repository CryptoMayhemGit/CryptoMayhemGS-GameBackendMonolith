using Mayhem.Wallet.Worker.Service.Interface;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.BackgroundWorker
{
    public class WalletTransferBackgroundWorker
    {
        private readonly IWorkerService workerService;

        public WalletTransferBackgroundWorker(
            IWorkerService workerService)
        {
            this.workerService = workerService;
        }

        [FunctionName("WalletUpdater")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
            ILogger log, ExecutionContext context)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"Start timer trigger function executed at: {DateTime.Now}");

            await workerService.UpdateAsync();

            log.LogInformation($"Stop timer trigger function executed at: {DateTime.Now}");
        }
    }
}
