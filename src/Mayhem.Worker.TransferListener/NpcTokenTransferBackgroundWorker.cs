using Mayhem.Blockchain.Enums;
using Mayhem.Configuration.Interfaces;
using Mayhem.Queue.Publisher;
using Mayhem.Worker.Base.Interfaces;
using Mayhem.Worker.TransferListener.Base;
using System.Timers;

namespace Mayhem.Worker.TransferListener
{
    public class NpcTokenTransferBackgroundWorker : BaseBackgroundWorker
    {
        private readonly INpcQueuePublisher npcQueuePublisher;
        private readonly IWorkerService workerService;

        public NpcTokenTransferBackgroundWorker(
            IMayhemConfigurationService mayhemConfigurationService,
            INpcQueuePublisher npcQueuePublisher,
            IWorkerService workerService) : base(mayhemConfigurationService)
        {
            this.npcQueuePublisher = npcQueuePublisher;
            this.workerService = workerService;
        }

        protected override async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await workerService.UpdateAsync(npcQueuePublisher, BlocksType.Npc);
        }
    }
}
