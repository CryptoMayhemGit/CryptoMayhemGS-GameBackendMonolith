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

namespace Mayhem.Land.QueueConsumer
{
    public class LandQueueConsumer : AzureQueueConsumer<LandQueueConsumer>
    {
        private readonly ILandRepository landRepository;
        public LandQueueConsumer(
            ILogger<LandQueueConsumer> logger,
            ILandQueueClient landQueueClient,
            ILandRepository landRepository)
            : base(logger, landQueueClient)
        {
            this.landRepository = landRepository;
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
                    bool result = await landRepository.UpdateLandOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                    if (result)
                    {
                        logger.LogDebug(LoggerMessages.UpdatedLandOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                    else
                    {
                        logger.LogWarning(LoggerMessages.CannotUpdateLandOwnerForWallet(eventMessage.To, eventMessage.Value));
                    }
                }
                else
                {
                    if (eventMessage.To.IsZeroAddress())
                    {
                        // burn
                        bool result = await landRepository.RemoveLandFromUserAsync(Convert.ToInt64(eventMessage.Value));
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.RemovedLandOwnerFor(eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotRemoveLandOwnerFor(eventMessage.Value));
                        }
                    }
                    else
                    {
                        // transfer from one player to another
                        bool result = await landRepository.UpdateLandOwnerAsync(Convert.ToInt64(eventMessage.Value), eventMessage.To);
                        if (result)
                        {
                            logger.LogDebug(LoggerMessages.UpdatedLandOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                        else
                        {
                            logger.LogWarning(LoggerMessages.CannotUpdateLandOwnerForWallet(eventMessage.To, eventMessage.Value));
                        }
                    }
                }
            }
        }
    }
}
