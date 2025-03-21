using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Mayhem.Nft.Worker.Service.Interface;
using System;

namespace Mayhem.Nft.Worker
{
    public class NftTransferBackgroundWorker
    {
        private readonly IWorkerService _workerService;

        public NftTransferBackgroundWorker(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [FunctionName("NftUpdater")]
        public async Task Run([TimerTrigger("0 0 10 * * *")] TimerInfo myTimer,
            ILogger log, ExecutionContext context)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"Start timer trigger function executed at: {DateTime.Now}");

            await _workerService.UpdateAsync();

            log.LogInformation($"Stop timer trigger function executed at: {DateTime.Now}");
        }
    }
}