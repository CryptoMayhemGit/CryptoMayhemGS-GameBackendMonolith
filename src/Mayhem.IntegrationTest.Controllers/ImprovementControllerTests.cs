using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Helper;
using Mayhem.IntegrationTest.Base;
using Mayhem.Test.Common;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.AddGuildImprovement;
using Mayhen.Bl.Commands.AddImprovement;
using Mayhen.Bl.Commands.CheckGuildImprovement;
using Mayhen.Bl.Commands.CheckImprovement;
using Mayhen.Bl.Commands.GetGuildImprovements;
using Mayhen.Bl.Commands.GetImprovements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class ImprovementControllerTests : ControllerTestBase<ImprovementControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public async Task Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();

            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == UserId);
            user.UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000000);
            await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = user.Id,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();
        }

        [Test]
        public async Task AddImprovement_WhenImprovementAdded_ThenGetIt_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}";

            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            AddImprovementCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                ImprovementTypeId = ImprovementsType.BasicMiningCombine
            };

            ActionDataResult<AddImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Improvement.Should().NotBeNull();
            response.Result.Improvement.Id.Should().BeGreaterThan(0);
            response.Result.Improvement.LandId.Should().Be(request.LandId);
            response.Result.Improvement.ImprovementTypeId.Should().Be(request.ImprovementTypeId);
        }

        [Test]
        public async Task AddGuildImprovement_WhenGuildImprovementAdded_ThenGetIt_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}/Guild";

            AddGuildImprovementCommandRequest request = new()
            {
                GuildId = 1,
                GuildImprovementTypeId = GuildImprovementsType.ImprovedAssemblyLine,
            };

            ActionDataResult<AddGuildImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.GuildImprovement.Should().NotBeNull();
            response.Result.GuildImprovement.Id.Should().BeGreaterThan(0);
            response.Result.GuildImprovement.GuildId.Should().Be(request.GuildId);
            response.Result.GuildImprovement.GuildImprovementTypeId.Should().Be(request.GuildImprovementTypeId);
        }

        [Test]
        public async Task AddImprovementTwice_WhenImprovementAdded_ThenGetBadRequest_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}";

            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            AddImprovementCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                ImprovementTypeId = ImprovementsType.DeepExcavationsTechnology
            };

            await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, Token);
            ActionDataResult<AddImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task AddGuildImprovementTwice_WhenGuildImprovementAdded_ThenGetBadRequest_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}/Guild";

            AddGuildImprovementCommandRequest request = new()
            {
                GuildId = 1,
                GuildImprovementTypeId = GuildImprovementsType.ImprovedTransmission
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, Token);
            ActionDataResult<AddGuildImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"Improvement with guildId {request.GuildId} and ImprovementsTypeId {request.GuildImprovementTypeId} already exists.");
            response.Errors.First().FieldName.Should().Be("GuildImprovement");
        }

        [Test]
        public async Task AddImprovementWithoutLand_WhenImprovementAdded_ThenGetBadRequest_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}";

            AddImprovementCommandRequest request = new()
            {
                LandId = 10000,
                ImprovementTypeId = ImprovementsType.BasicMiningCombine
            };

            await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, Token);
            ActionDataResult<AddImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task AddGuildImprovementWithoutGuild_WhenGuildImprovementAdded_ThenGetBadRequest_Test()
        {
            string endpoint = $"api/{ControllerNames.Improvement}/Guild";

            AddGuildImprovementCommandRequest request = new()
            {
                GuildId = 10000,
                GuildImprovementTypeId = GuildImprovementsType.ImprovedTransmission
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, Token);
            ActionDataResult<AddGuildImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be("Guild with id 10000 doesn't exist.");
            response.Errors.First().FieldName.Should().Be("GuildId");
        }

        [Test]
        public async Task CheckImprovement_WhenImprovementCanBeAdded_ThenReturnSuccess_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            await AddImprovementsToDb(newLand.Entity.Id);


            CheckImprovementCommandRequest request = new()
            {
                Level = 2,
                BuildingsTypeId = BuildingsType.Lumbermill,
                LandId = newLand.Entity.Id,
            };

            string endpoint = $"api/{ControllerNames.Improvement}/Check?{request.ToQueryString()}";

            ActionDataResult<CheckImprovementCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckImprovementCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.CanImprove.Should().BeTrue();
        }

        [Test]
        public async Task CheckGuildImprovement_WhenGuildImprovementCanBeAdded_ThenReturnSuccess_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> newGuild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            await AddGuildImprovementsToDb(newGuild.Entity.Id);

            CheckGuildImprovementCommandRequest request = new()
            {
                Level = 2,
                GuildBuildingsTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
                GuildId = newGuild.Entity.Id,
            };

            string endpoint = $"api/{ControllerNames.Improvement}/Guild/Check?{request.ToQueryString()}";

            ActionDataResult<CheckGuildImprovementCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckGuildImprovementCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.CanImprove.Should().BeTrue();
        }

        [Test]
        public async Task CheckImprovement_WhenImprovementCantBeAdded_ThenReturnFailure_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            CheckImprovementCommandRequest request = new()
            {
                Level = 2,
                BuildingsTypeId = BuildingsType.Lumbermill,
                LandId = newLand.Entity.Id,
            };

            string endpoint = $"api/{ControllerNames.Improvement}/Check?{request.ToQueryString()}";

            ActionDataResult<CheckImprovementCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckImprovementCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.CanImprove.Should().BeFalse();
        }

        [Test]
        public async Task CheckGuildImprovement_WhenGuildImprovementCantBeAdded_ThenReturnFailure_Test()
        {
            CheckGuildImprovementCommandRequest request = new()
            {
                Level = 2,
                GuildBuildingsTypeId = GuildBuildingsType.FightBoard,
                GuildId = 1,
            };

            string endpoint = $"api/{ControllerNames.Improvement}/Guild/Check?{request.ToQueryString()}";

            ActionDataResult<CheckGuildImprovementCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckGuildImprovementCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.CanImprove.Should().BeFalse();
        }

        [Test]
        public async Task GetImprovements_WhenImprovementNotExists_ThenGetEmptyCollection_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Improvement}/{newLand.Entity.Id}";

            ActionDataResult<GetImprovementsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetImprovementsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Improvements.Should().BeEmpty();
        }

        [Test]
        public async Task GetGuildImprovements_WhenGuildImprovementNotExists_ThenGetEmptyCollection_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();

            EntityEntry<Guild> newGuild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Improvement}/Guild/{newGuild.Entity.Id}";

            ActionDataResult<GetGuildImprovementsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetGuildImprovementsCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.GuildImprovements.Should().BeEmpty();
        }

        [Test]
        public async Task GetImprovements_WhenImprovementExists_ThenGetThem_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            await AddImprovementsToDb(newLand.Entity.Id);

            string endpoint = $"api/{ControllerNames.Improvement}/{newLand.Entity.Id}";

            ActionDataResult<GetImprovementsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetImprovementsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Improvements.Should().HaveCount(3);
        }

        [Test]
        public async Task GetGuildImprovements_WhenGuildImprovementExists_ThenGetThem_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();

            EntityEntry<Guild> newGuild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
            });
            await mayhemDataContext.SaveChangesAsync();

            await AddGuildImprovementsToDb(newGuild.Entity.Id);

            string endpoint = $"api/{ControllerNames.Improvement}/Guild/{newGuild.Entity.Id}";

            ActionDataResult<GetGuildImprovementsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetGuildImprovementsCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.GuildImprovements.Should().HaveCount(3);
        }

        [Test]
        public async Task AddImpovement_WhenUserDoesntHaveResource_ThenGetValidationErrors_Test()
        {
            (_, string token) = await GetNewTokenAsync();
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());

            string endpoint = $"api/{ControllerNames.Improvement}";

            AddImprovementCommandRequest request = new()
            {
                ImprovementTypeId = ImprovementsType.AdditionalTankForMechanium,
                LandId = land.Entity.Id,
            };

            ActionDataResult<AddImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddImprovementCommandRequest, AddImprovementCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task AddGuildImpovement_WhenGuildDoesnotHaveResource_ThenGetValidationErrors_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(0),
                OwnerId = userId,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Improvement}/Guild";

            AddGuildImprovementCommandRequest request = new()
            {
                GuildImprovementTypeId = GuildImprovementsType.ImprovedTransmission,
                GuildId = guild.Entity.Id,
            };

            ActionDataResult<AddGuildImprovementCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        private async Task AddImprovementsToDb(long landId)
        {
            await mayhemDataContext.Improvements.AddAsync(new Improvement()
            {
                LandId = landId,
                ImprovementTypeId = ImprovementsType.ReinforcedChainsawMotor
            });
            await mayhemDataContext.Improvements.AddAsync(new Improvement()
            {
                LandId = landId,
                ImprovementTypeId = ImprovementsType.ImprovedGear
            });
            await mayhemDataContext.Improvements.AddAsync(new Improvement()
            {
                LandId = landId,
                ImprovementTypeId = ImprovementsType.HardenedSawChain
            });

            await mayhemDataContext.SaveChangesAsync();
        }

        private async Task AddGuildImprovementsToDb(int guildId)
        {
            await mayhemDataContext.GuildImprovements.AddAsync(new GuildImprovement()
            {
                GuildId = guildId,
                GuildImprovementTypeId = GuildImprovementsType.RegenerativeMeal
            });
            await mayhemDataContext.GuildImprovements.AddAsync(new GuildImprovement()
            {
                GuildId = guildId,
                GuildImprovementTypeId = GuildImprovementsType.Flashlight
            });
            await mayhemDataContext.GuildImprovements.AddAsync(new GuildImprovement()
            {
                GuildId = guildId,
                GuildImprovementTypeId = GuildImprovementsType.Motivator
            });


            await mayhemDataContext.SaveChangesAsync();
        }
    }
}