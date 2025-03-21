using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AskToJoinGuildByUserCommandRequestValidatorTests : UnitTestBase
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
        public async Task AsksToJoinGuildByUser_WhenUserNotExist_ThenThrowException_Test()
        {
            int userId = 34259;
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);

            AskToJoinGuildByUserCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AskToJoinGuildByUserCommandRequest()
            {
                GuildId = guild.Id,
                UserId = userId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {userId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenUserIsInOtherGuild_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AddUserToGuildAsync(guild.Id, user.Entity.Id);

            AskToJoinGuildByUserCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AskToJoinGuildByUserCommandRequest()
            {
                GuildId = guild.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User with id {user.Entity.Id} is already in other guild.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenGuildNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            AskToJoinGuildByUserCommandRequestValidator validator = new(mayhemDataContext);
            AskToJoinGuildByUserCommandRequest request = new()
            {
                GuildId = 12342,
                UserId = user.Entity.Id,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild with id {request.GuildId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"GuildId");
        }

        [Test]
        public async Task AsksToJoinGuildByUser_WhenGuildAlreadyReceivedInquiry_ThenThrowException_Test()
        {
            EntityEntry<GameUser> owner = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            GuildDto guild = await guildRepository.CreateGuildAsync(Guid.NewGuid().ToString(), "desc", owner.Entity.Id);
            await guildRepository.AskToJoinGuildByUserAsync(guild.Id, user.Entity.Id);

            AskToJoinGuildByUserCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AskToJoinGuildByUserCommandRequest()
            {
                GuildId = guild.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"User has already sent an invitation.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }
    }
}
