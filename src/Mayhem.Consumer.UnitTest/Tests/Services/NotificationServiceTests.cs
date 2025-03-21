using FluentAssertions;
using Mayhem.Consumer.Test.Common.Builder;
using Mayhem.Consumer.UnitTest.Base;
using Mayhem.SmtpBase.Services.Interfaces;
using Mayhem.SmtpServices.Dtos;
using Mayhem.SmtpServices.Interfaces;
using Mayhem.SmtpServices.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Mayhem.Consumer.UnitTest.Tests.Services
{
    public class NotificationServiceTests : BaseRepositoryTests
    {
        [Test]
        public async Task SendNotificationAsync_WhenSmtpClientSend_ThenGetTrue_Test()
        {
            const bool expectedResult = true;
            EmailNotificationDto expectedNotificationDto = new()
            {
                Id = 1,
                Email = "test@adriagames.com",
                WalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D"
            };

            ISmtpService smtpService = SmtpServiceMockBuilder
                .Create()
                .WithSendAsync(expectedResult)
                .Build();

            IInviteNotificationService home = new InviteNotificationService(smtpService, mayhemConfiguration);

            bool result = await home.SendNotificationAsync(expectedNotificationDto);

            result.Should().Be(expectedResult);
        }


        [Test]
        public async Task SendNotificationAsync_WhenSmtpClientNotSend_ThenGetFalse_Test()
        {
            const bool expectedResult = false;
            EmailNotificationDto expectedNotificationDto = new()
            {
                Id = 1,
                Email = "test@adriagames.com",
                WalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D"
            };
            ISmtpService smtpService = SmtpServiceMockBuilder
                .Create()
                .WithSendAsync(expectedResult)
                .Build();

            IInviteNotificationService home = new InviteNotificationService(smtpService, mayhemConfiguration);

            bool result = await home.SendNotificationAsync(expectedNotificationDto);

            result.Should().Be(expectedResult);
        }

        [Test]
        public async Task SendNotificationAsync_WhenSmtpClientThrow_ThenGetFalse_Test()
        {
            EmailNotificationDto expectedNotificationDto = new()
            {
                Id = 1,
                Email = "test@adriagames.com",
                WalletAddress = "18qkmtiQkenExFLNUNF5WUpzZqKu7T9h5D"
            };

            ISmtpService smtpService = SmtpServiceMockBuilder
                .Create()
                .WithSendAsyncThrow()
                .Build();

            IInviteNotificationService home = new InviteNotificationService(smtpService, mayhemConfiguration);

            Func<Task<bool>> function = () => home.SendNotificationAsync(expectedNotificationDto);

            await function.Should().ThrowAsync<Exception>();
        }
    }
}