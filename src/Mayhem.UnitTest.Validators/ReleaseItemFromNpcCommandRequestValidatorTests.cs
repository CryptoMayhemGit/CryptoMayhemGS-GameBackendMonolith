using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.ReleaseItemFromNpc;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class ReleaseItemFromNpcCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task ReleaseItemFromNpc_WhenItemNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());

            ReleaseItemFromNpcCommandRequestValidator validator = new(mayhemDataContext);
            ReleaseItemFromNpcCommandRequest request = new()
            {
                ItemId = 1234,
                UserId = user.Entity.Id,
            };
            ValidationResult result = validator.Validate(request);


            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Item with id {request.ItemId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"ItemId");
        }
    }
}
