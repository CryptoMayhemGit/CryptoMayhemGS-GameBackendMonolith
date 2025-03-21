using Microsoft.Azure.ServiceBus;
using Moq;

namespace Mayhem.Consumer.Test.Common.Builder
{
    public static class QueueClientMockBuilder
    {
        public static Mock<IQueueClient> Create()
        {
            return new Mock<IQueueClient>();
        }

        public static IQueueClient Build(this Mock<IQueueClient> logger)
        {
            return logger.Object;
        }
    }
}
