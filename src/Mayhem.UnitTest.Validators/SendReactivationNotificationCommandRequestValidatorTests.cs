using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.SendReativationNotification;
using Mayhen.Bl.Validators;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class SendReactivationNotificationCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IMayhemConfigurationService mayhemConfigurationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            mayhemConfigurationService = GetService<IMayhemConfigurationService>();
        }

        [Test]
        public async Task SendReactivationNotification_WhenSendTimeIsLowerThenTimeFromConfiguration_ThenGetValidationError_Test()
        {
            int resendTime = mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.ResendNotificationTimeInMinutes;
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@adria.com";
            await mayhemDataContext.Notifications.AddAsync(new Notification()
            {
                Email = email,
                LastModificationDate = DateTime.UtcNow,
            });
            await mayhemDataContext.SaveChangesAsync();

            SendReactivationNotificationCommandRequestValidator validator = new(mayhemConfigurationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new SendReactivationNotificationCommandRequest()
            {
                Email = email,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Please wait {resendTime} minutes before resending");
            result.Errors.First().PropertyName.Should().Be("ResendTime");
        }

        [Test]
        public void SendReactivationNotification_WhenNotificationNotExist_ThenGetValidationError_Test()
        {
            string email = $"{Guid.NewGuid().ToString().Replace("-", "")}@adria.com";

            SendReactivationNotificationCommandRequestValidator validator = new(mayhemConfigurationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new SendReactivationNotificationCommandRequest()
            {
                Email = email,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Notification with email {email} does not exist.");
            result.Errors.First().PropertyName.Should().Be("NotificationId");
        }
    }
}
