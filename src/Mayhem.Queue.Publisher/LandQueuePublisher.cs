using Mayhem.Queue.Interfaces;
using Mayhem.Queue.Publisher.Base.Interfaces;
using Mayhem.Queue.Publisher.Base.Services;

namespace Mayhem.Queue.Publisher
{
    public class LandQueuePublisher : AzureServiceBusService, ILandQueuePublisher
    {
        public LandQueuePublisher(IQueueConfiguration queueConfiguration) : base(queueConfiguration)
        {
        }
    }

    public interface ILandQueuePublisher : IQueueService
    {

    }
}
