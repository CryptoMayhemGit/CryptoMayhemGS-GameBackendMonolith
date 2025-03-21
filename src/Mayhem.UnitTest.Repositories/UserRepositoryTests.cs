using FluentAssertions;
using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class UserRepositoryTests : UnitTestBase
    {
        private IUserRepository userRepository;
        private IMayhemDataContext mayhemDataContext;
        private const string expectedEmail = "test@adria.pl";

        [SetUp]
        public void Setup()
        {
            userRepository = GetService<IUserRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task CreateUserAsync_WhenUserCreated_ThenReturnTrue_Test()
        {
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");

            int? userId = await userRepository.CreateUserAsync(walletAddress, expectedEmail);

            List<GameUser> accountList = mayhemDataContext.GameUsers.Where(x => x.WalletAddress.Equals(walletAddress)).ToList();

            userId.Should().HaveValue();
            accountList.Should().HaveCount(1);
        }

        [Test]
        public async Task LoginAsync_WhenUserExists_ThenChangeLastLoginDate_Test()
        {
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");

            int? userId = await userRepository.CreateUserAsync(walletAddress, expectedEmail);
            await userRepository.LoginAsync(walletAddress);

            GameUser loggedUser = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);

            loggedUser.LastLoginDate.Should().BeAtLeast(DateTime.UtcNow.AddSeconds(-10).TimeOfDay);
        }

        [Test]
        public async Task GetUserAsync_WhenUserExist_ThenGetIt_Test()
        {
            string walletAddress = Guid.NewGuid().ToString().Replace("-", "");
            int? userId = await userRepository.CreateUserAsync(walletAddress, expectedEmail);

            await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = userId,
                ItemTypeId = ItemsType.AirHammer,
            });

            await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = userId, NpcTypeId = NpcsType.Doctor });

            await mayhemDataContext.Lands.AddAsync(new Land()
            {
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        Owned = true,
                        UserId = userId.Value,
                    },
                }
            });

            await mayhemDataContext.SaveChangesAsync();

            GetUserCommandResponseDto response = await userRepository.GetUserAsync(new GetUserCommandRequestDto()
            {
                UserId = userId.Value,
                WithItems = true,
                WithLands = true,
                WithNpcs = true,
                WithResources = true,
            });

            response.GameUser.Should().NotBeNull();
            response.GameUser.Id.Should().Be(userId);
            response.UserResources.Should().HaveCount(7);
            response.Npcs.Should().HaveCount(1);
            response.UserLands.Should().HaveCount(1);
            response.Items.Should().HaveCount(1);

        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsNotUsed_ThetReturnFalse_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";
            bool result = await userRepository.CheckEmailAsync(email);

            result.Should().BeFalse();
        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsInNotification_ThetReturnTrue_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";
            await mayhemDataContext.Notifications.AddAsync(new Notification()
            {
                Email = email,
            });

            await mayhemDataContext.SaveChangesAsync();

            bool result = await userRepository.CheckEmailAsync(email);

            result.Should().BeTrue();
        }

        [Test]
        public async Task CheckUserEmail_WhenEmailIsInUser_ThetReturnTrue_Test()
        {
            string email = $"{Guid.NewGuid()}@email.com";
            await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Email = email,
            });

            await mayhemDataContext.SaveChangesAsync();

            bool result = await userRepository.CheckEmailAsync(email);

            result.Should().BeTrue();
        }
    }
}