using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class UserLandRepositoryTests : UnitTestBase
    {
        private IUserLandRepository userLandRepository;
        private IMayhemDataContext mayhemDataContext;

        private static List<CheckPurchaseLandTestDto> CheckPurchaseLandCases()
        {
            return new List<CheckPurchaseLandTestDto>()
            {
                new CheckPurchaseLandTestDto(1, 1, 1, false, LandsStatus.Explored, false, true),
                new CheckPurchaseLandTestDto(1, 1, 1, false, LandsStatus.Discovered, false, false),
                new CheckPurchaseLandTestDto(1, 1, 1, false, LandsStatus.None, false, false),
                new CheckPurchaseLandTestDto(1, 1, 1, true, LandsStatus.Explored, false, false),
                new CheckPurchaseLandTestDto(1, 1, 1, true, LandsStatus.Discovered, false, false),
                new CheckPurchaseLandTestDto(1, 1, 1, true, LandsStatus.None, false, false),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.Explored, true, true),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.Discovered, true, true),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.None, true, true),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.Explored, false, false),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.Discovered, false, false),
                new CheckPurchaseLandTestDto(2, 1, 1, true, LandsStatus.None, false, false),
                new CheckPurchaseLandTestDto(2, 1, 1, false, LandsStatus.Explored, true, false),
                new CheckPurchaseLandTestDto(2, 1, 1, false, LandsStatus.Discovered, true, false),
                new CheckPurchaseLandTestDto(2, 1, 1, false, LandsStatus.None, true, false),
            };
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            userLandRepository = GetService<IUserLandRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetLandById_WhenLandExist_ThemGetIt_Test()
        {
            EntityEntry<UserLand> newUserLand = await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                Land = new Land()
            });
            await mayhemDataContext.SaveChangesAsync();

            UserLandDto userLand = await userLandRepository.GetUserLandAsync(newUserLand.Entity.Id);

            userLand.Should().NotBeNull();
        }

        [Test]
        public async Task GetLandById_WhenLandNotExist_ThemGetNull_Test()
        {
            UserLandDto userLand = await userLandRepository.GetUserLandAsync(1234);

            userLand.Should().BeNull();
        }

        [Test]
        public async Task DiscoverLand_WhenLandDiscovered_ThenGetIt_Test()
        {
            EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();
            UserLandDto userLand = await userLandRepository.DiscoverUserLandAsync(newLand.Entity.Id, newGameUser.Entity.Id);

            UserLand userLandDb = await mayhemDataContext.UserLands.Where(x => x.LandId == newLand.Entity.Id && x.UserId == newGameUser.Entity.Id).SingleOrDefaultAsync();

            userLand.Should().NotBeNull();
            userLandDb.Should().NotBeNull();
            userLandDb.Status.Should().Be(LandsStatus.Discovered);
        }

        [Test, Order(2)]
        public async Task DiscoverLandWhereUserIsOwnerAndPositionYIsOdd_WhenLandDiscovered_ThenGetIt_Test()
        {
            EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = j, PositionY = i });
                }
            }
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 1, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 2, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 6, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 7, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.None });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 3, UserId = newGameUser.Entity.Id, HasFog = false });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 8, UserId = newGameUser.Entity.Id, HasFog = true });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 11, UserId = newGameUser.Entity.Id, HasFog = false });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 12, UserId = newGameUser.Entity.Id, HasFog = false });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 13, UserId = newGameUser.Entity.Id, HasFog = true });
            await mayhemDataContext.SaveChangesAsync();

            UserLandDto userLand = await userLandRepository.DiscoverUserLandAsync(7, newGameUser.Entity.Id);

            List<UserLand> userLandsDb = await mayhemDataContext.UserLands.Where(x => (x.LandId == 8 || x.LandId == 13) && x.UserId == newGameUser.Entity.Id).ToListAsync();

            userLand.Should().NotBeNull();
            userLandsDb.All(x => x.HasFog == false).Should().BeTrue();
        }

        [Test, Order(1)]
        public async Task DiscoverLandWhereUserIsOwnerAndPositionYIsEven_WhenLandDiscovered_ThenGetIt_Test()
        {
            EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = j, PositionY = i });
                }
            }
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 7, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 8, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 12, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 13, UserId = newGameUser.Entity.Id, Owned = true, Status = LandsStatus.None });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 14, UserId = newGameUser.Entity.Id, HasFog = true });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 17, UserId = newGameUser.Entity.Id, HasFog = true });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = 18, UserId = newGameUser.Entity.Id, HasFog = true });
            await mayhemDataContext.SaveChangesAsync();

            UserLandDto userLand = await userLandRepository.DiscoverUserLandAsync(13, newGameUser.Entity.Id);

            List<UserLand> userLandsDb = await mayhemDataContext.UserLands.Where(x => (x.LandId == 14 || x.LandId == 17 || x.LandId == 18) && x.UserId == newGameUser.Entity.Id).ToListAsync();

            userLand.Should().NotBeNull();
            userLandsDb.All(x => x.HasFog == false).Should().BeTrue();
        }

        [Test]
        public async Task DiscoverLand_WhenUserLandExist_ThenUpdateIdAndGetIt_Test()
        {
            EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { UserId = newGameUser.Entity.Id, LandId = newLand.Entity.Id, Status = LandsStatus.None });
            await mayhemDataContext.SaveChangesAsync();

            UserLandDto userLand = await userLandRepository.DiscoverUserLandAsync(newLand.Entity.Id, newGameUser.Entity.Id);

            UserLand userLandDb = await mayhemDataContext.UserLands.Where(x => x.LandId == newLand.Entity.Id && x.UserId == newGameUser.Entity.Id).SingleOrDefaultAsync();

            userLand.Should().NotBeNull();
            userLandDb.Should().NotBeNull();
            userLandDb.Status.Should().Be(LandsStatus.Discovered);
        }

        [Test]
        public async Task ExploreLand_WhenLandExplored_ThenGetIt_Test()
        {
            EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { UserId = newGameUser.Entity.Id, LandId = newLand.Entity.Id, Status = LandsStatus.Discovered });
            await mayhemDataContext.SaveChangesAsync();
            UserLandDto userLand = await userLandRepository.ExploreUserLandAsync(newLand.Entity.Id, newGameUser.Entity.Id);

            UserLand userLandDb = await mayhemDataContext.UserLands.Where(x => x.LandId == newLand.Entity.Id && x.UserId == newGameUser.Entity.Id).SingleOrDefaultAsync();

            userLand.Should().NotBeNull();
            userLandDb.Should().NotBeNull();
            userLandDb.Status.Should().Be(LandsStatus.Explored);
        }

        [Test]
        public async Task GetUserLands_WhenLandsExist_ThenGetThem_Test()
        {
            const int expectedLandsCount = 7;

            EntityEntry<GameUser> user1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            for (int i = 0; i < expectedLandsCount; i++)
            {
                await mayhemDataContext.UserLands.AddAsync(new UserLand() { UserId = user1.Entity.Id, Land = newLand.Entity });
            }

            await mayhemDataContext.UserLands.AddAsync(new UserLand() { UserId = user2.Entity.Id, Land = newLand.Entity });
            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<UserLandDto> userLands = await userLandRepository.GetUserLandsAsync(user1.Entity.Id);

            userLands.Should().HaveCount(expectedLandsCount);
            userLands.All(x => x.Land != null).Should().BeTrue();
        }

        [TestCaseSource(nameof(CheckPurchaseLandCases)), Order(1)]
        public async Task CheckPurchaseLand_WhenCanPurchase_ThemGetTrue_Test(CheckPurchaseLandTestDto @case)
        {
            EntityEntry<UserLand> userLand = await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                LandId = @case.UserLand.LandId,
                UserId = @case.UserLand.UserId,
                Status = @case.UserLand.Status,
                Owned = @case.UserLand.Owned,
                OnSale = @case.UserLand.OnSale,
            });

            await mayhemDataContext.SaveChangesAsync();

            bool result = await userLandRepository.CheckPurchaseLandAsync(@case.UserLand.LandId, @case.UserPerspectiveId);

            result.Should().Be(@case.Result);

            mayhemDataContext.UserLands.Remove(userLand.Entity);
            await mayhemDataContext.SaveChangesAsync();
        }

        private async Task CreateLandsAsync(int landInstanceId)
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    await mayhemDataContext.Lands.AddAsync(new Land()
                    {
                        PositionX = j,
                        PositionY = i,
                        LandInstanceId = landInstanceId,
                    });

                    await mayhemDataContext.SaveChangesAsync();
                }
            }
        }
    }

    public class CheckPurchaseLandTestDto
    {
        public UserLand UserLand { get; set; }
        public bool Result { get; set; }
        public int UserPerspectiveId { get; set; }

        public CheckPurchaseLandTestDto(int userPerspectiveId, int userId, long landId, bool owned, LandsStatus status, bool onSale, bool result)
        {
            UserPerspectiveId = userPerspectiveId;
            UserLand = new()
            {
                UserId = userId,
                LandId = landId,
                Owned = owned,
                Status = status,
                OnSale = onSale
            };
            Result = result;
        }
    }
}
