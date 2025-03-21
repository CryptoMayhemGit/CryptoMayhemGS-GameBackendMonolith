using Mayhem.Blockchain.Enums;
using Mayhem.Configuration.Interfaces;
using Mayhem.Queue.Publisher;
using Mayhem.Worker.Base.Interfaces;
using Mayhem.Worker.TransferListener.Base;
using System.Timers;

namespace Mayhem.Worker.TransferListener
{
    public class ItemTokenTransferBackgroundWorker : BaseBackgroundWorker
    {
        private readonly IItemQueuePublisher itemQueuePublisher;
        private readonly IWorkerService workerService;

        public ItemTokenTransferBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            IItemQueuePublisher itemQueuePublisher,
            IWorkerService workerService) : base(mayhemConfigurationService)
        {
            this.itemQueuePublisher = itemQueuePublisher;
            this.workerService = workerService;
        }

        protected override async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await workerService.UpdateAsync(itemQueuePublisher, BlocksType.Item);
        }
    }
}
