using Mayhem.Blockchain.Enums;
using Mayhem.Configuration.Interfaces;
using Mayhem.Queue.Publisher;
using Mayhem.Worker.Base.Interfaces;
using Mayhem.Worker.TransferListener.Base;
using System.Timers;

namespace Mayhem.Worker.TransferListener
{
    public class LandTokenTransferBackgroundWorker : BaseBackgroundWorker
    {
        private readonly IWorkerService workerService;
        private readonly ILandQueuePublisher landQueuePublisher;

        public LandTokenTransferBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            ILandQueuePublisher landQueuePublisher,
            IWorkerService workerService) : base(mayhemConfigurationService)
        {
            this.landQueuePublisher = landQueuePublisher;
            this.workerService = workerService;
        }

        protected override async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await workerService.UpdateAsync(landQueuePublisher, BlocksType.Land);
        }
    }
}
