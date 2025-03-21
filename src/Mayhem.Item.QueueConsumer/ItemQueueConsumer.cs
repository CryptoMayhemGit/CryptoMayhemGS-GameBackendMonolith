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

namespace Mayhem.Item.QueueConsumer
{
    public class ItemQueueConsumer : AzureQueueConsumer<ItemQueueConsumer>
    {
        private readonly IItemRepository itemRepository;

        public ItemQueueConsumer(ILogger<ItemQueueConsumer> logger,
                                 IItemQueueClient itemQueueClient,
                                 IItemRepository itemRepository)
            : base(logger, itemQueueClient)
        {
            this.itemRepository = itemRepository;
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
                    bool result = await itemRepository.UpdateItemOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                    if (result)
                    {
                        logger.LogDebug(LoggerMessages.UpdatedItemOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                    else
                    {
                        logger.LogWarning(LoggerMessages.CannotUpdateItemOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                }
                else
                {
                    if (eventMessage.To.IsZeroAddress())
                    {
                        // burn
                        bool result = await itemRepository.RemoveItemFromUserAsync(Convert.ToInt64(eventMessage.Value));
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.RemovedItemOwnerFor(eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotRemoveItemOwnerFor(eventMessage.Value));
                        }
                    }
                    else
                    {
                        // transfer from one player to another
                        bool result = await itemRepository.UpdateItemOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.UpdatedItemOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotUpdateItemOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                    }
                }
            }
        }
    }
}
