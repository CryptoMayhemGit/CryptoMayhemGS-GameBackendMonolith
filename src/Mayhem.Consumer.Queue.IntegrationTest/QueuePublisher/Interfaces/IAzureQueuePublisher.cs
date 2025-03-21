using System.Threading.Tasks;

namespace Mayhem.Consumer.Queue.IntegrationTest.QueuePublisher.Interfaces
{
    public interface IAzureQueuePublisher
    {
        Task<bool> PublishMessage(object message);
    }
}
