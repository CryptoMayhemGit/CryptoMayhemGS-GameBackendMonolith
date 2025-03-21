using Mayhem.Blockchain.Helpers;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhem.Queue.Consumer.Base.Services;
using Mayhem.Queue.Dto;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhem.Npc.QueueConsumer
{
    public class NpcQueueConsumer : AzureQueueConsumer<NpcQueueConsumer>
    {
        private readonly INpcRepository npcRepository;

        public NpcQueueConsumer(ILogger<NpcQueueConsumer> logger,
                                INpcQueueClient npcQueueClient,
                                INpcRepository npcRepository)
            : base(logger, npcQueueClient)
        {
            this.npcRepository = npcRepository;
        }

        public override async Task ProcessMessagesAsync(Message message, CancellationToken token = default)
        {
            TransferQueueMessage transferEventMessage = FromMessage<TransferQueueMessage>(message);

            foreach (TransferQueueDto eventMessage in transferEventMessage.Dtos)
            {
                logger.LogDebug(LoggerMessages.ReceivedMessageFrom(eventMessage.From, eventMessage.To, eventMessage.Value));

                if (eventMessage.From.IsZeroAddress())
                {
                    // mint
                    bool result = await npcRepository.UpdateNpcOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                    if (result)
                    {
                        logger.LogDebug(LoggerMessages.UpdatedNpcOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                    else
                    {
                        logger.LogWarning(LoggerMessages.CannotUpdateNpcOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                }
                else
                {
                    if (eventMessage.To.IsZeroAddress())
                    {
                        // burn
                        bool result = await npcRepository.RemoveNpcFromUserAsync(Convert.ToInt64(eventMessage.Value));
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.RemovedNpcOwnerFor(eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotRemoveNpcOwnerFor(eventMessage.Value));
                        }
                    }
                    else
                    {
                        // transfer from one player to another
                        bool result = await npcRepository.UpdateNpcOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.UpdatedNpcOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotUpdateNpcOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                    }
                }
            }
        }
    }
}
