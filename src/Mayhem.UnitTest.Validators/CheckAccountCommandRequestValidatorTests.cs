using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.CheckAccount;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class CheckAccountCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task CheckAccount_WhenAccountExist_ThenThrowException_Test()
        {
            const string expectedAddres = "1AyTZkemoKAKEfakYcAy5vHpJj7QbZVHS1";
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser() { WalletAddress = expectedAddres });
            await mayhemDataContext.SaveChangesAsync();

            CheckAccountCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CheckAccountCommandRequest()
            {
                WalletAddress = user.Entity.WalletAddress
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Account with wallet {expectedAddres} already exists.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task CheckAccount_WhenNotificationsExist_ThenThrowException_Test()
        {
            const string expectedAddres = "12fAs3E1q6j1Xotzo8gi3XwWuYFhQ8cpNC";
            EntityEntry<Notification> notification = await mayhemDataContext.Notifications.AddAsync(new Notification() { WalletAddress = "12fAs3E1q6j1Xotzo8gi3XwWuYFhQ8cpNC" });
            await mayhemDataContext.SaveChangesAsync();

            CheckAccountCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CheckAccountCommandRequest()
            {
                WalletAddress = notification.Entity.WalletAddress
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Notification with wallet {expectedAddres} already exists.");
            result.Errors.First().PropertyName.Should().Be($"NotificationId");
        }
    }
}
