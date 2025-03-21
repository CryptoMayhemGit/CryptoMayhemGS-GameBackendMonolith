using AutoMapper;
using Mayhem.Dal.Dto.Commands.SendActivationNotification;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public NotificationRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<int> AddNotificationAsync(SendActivationNotificationCommandRequestDto sendActivationNotificationCommandRequest)
        {
            Notification newNotification = new()
            {
                WalletAddress = sendActivationNotificationCommandRequest.Wallet,
                Email = sendActivationNotificationCommandRequest.Email
            };

            await mayhemDataContext.Notifications.AddAsync(newNotification);
            await mayhemDataContext.SaveChangesAsync();

            return await Task.FromResult(newNotification.Id);
        }

        public async Task<bool> CheckActivationLinkAsync(string wallet, string email)
        {
            Notification notification = await mayhemDataContext
                .Notifications
                .Where(x => x.WalletAddress.Equals(wallet)
                && x.Email == email)
                .SingleOrDefaultAsync();

            mayhemDataContext.Notifications.Remove(notification);
            await mayhemDataContext.SaveChangesAsync();

            return await Task.FromResult(true);
        }

        public async Task<NotificationDto> GetNotificationByEmailAsync(string email) => await mayhemDataContext
                .Notifications
                .AsNoTracking()
                .Where(x => x.Email == email)
                .Select(x => mapper.Map<NotificationDto>(x))
                .SingleOrDefaultAsync();

        public async Task UpdateNotificationDate(int notificationId)
        {
            Notification notification = await mayhemDataContext
                .Notifications
                .SingleOrDefaultAsync(x => x.Id == notificationId);

            notification.LastModificationDate = DateTime.UtcNow;

            await mayhemDataContext.SaveChangesAsync();
        }
    }
}
