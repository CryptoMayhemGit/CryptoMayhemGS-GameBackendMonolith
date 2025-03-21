using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class CreateGuildCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IGuildRepository guildRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            guildRepository = GetService<IGuildRepository>();
        }

        [Test]
        public async Task CreateGuild_WhenGuildWithThisNameExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            string guildName = Guid.NewGuid().ToString();
            await guildRepository.CreateGuildAsync(guildName, "my guild description", user.Entity.Id);

            CreateGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CreateGuildCommandRequest()
            {
                Description = "my guild description",
                Name = guildName,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild with name {guildName} already exists.");
            result.Errors.First().PropertyName.Should().Be($"GuildName");
        }

        [Test]
        public void CreateGuild_WhenOwnerNotExist_ThenThrowException_Test()
        {
            int userId = 3653;

            CreateGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CreateGuildCommandRequest()
            {
                Description = "my guild description",
                Name = Guid.NewGuid().ToString(),
                UserId = userId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {userId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task CreateGuild_WhenOwnerHasOtherGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "my guild description", user.Entity.Id);

            CreateGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CreateGuildCommandRequest()
            {
                Description = "my guild description",
                Name = Guid.NewGuid().ToString(),
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {user.Entity.Id} already has guild.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }
    }
}
