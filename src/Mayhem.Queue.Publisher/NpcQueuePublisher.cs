﻿using Mayhem.Queue.Interfaces;
using Mayhem.Queue.Publisher.Base.Interfaces;
using Mayhem.Queue.Publisher.Base.Services;

namespace Mayhem.Queue.Publisher
{
    public class NpcQueuePublisher : AzureServiceBusService, INpcQueuePublisher
    {
        public NpcQueuePublisher(IQueueConfiguration queueConfiguration) : base(queueConfiguration)
        {
        }
    }

    public interface INpcQueuePublisher : IQueueService
    {

    }
}
