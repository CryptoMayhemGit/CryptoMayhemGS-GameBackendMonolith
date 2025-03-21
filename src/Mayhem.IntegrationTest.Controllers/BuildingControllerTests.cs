using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Test.Common;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.AddBuildingToLand;
using Mayhen.Bl.Commands.AddGuildBuilding;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Commands.GetBuildingList;
using Mayhen.Bl.Commands.GetGuildBuildingList;
using Mayhen.Bl.Commands.UpgradeBuilding;
using Mayhen.Bl.Commands.UpgradeGuildBuilding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class BuildingControllerTests : GuildTestBase<BuildingControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;
        private IImprovementRepository improvementRepository;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            improvementRepository = GetService<IImprovementRepository>();

            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == UserId);
            user.UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000000);
            await mayhemDataContext.SaveChangesAsync();
        }

        public async Task AddBuildingToLand_WhenBuildingAdded_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = UserId,
                    }
                }
            });
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = UserId,
                Land = newLand.Entity,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.DroneFactory,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Building.Should().NotBeNull();
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildBuildingAdded_ThenGetIt_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";

            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> response =
                await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.GuildBuilding.Should().NotBeNull();
        }

        [Test]
        public async Task AddBuildingToLand_WhenLandNotExist_ThenGetNotFound_Test()
        {
            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = 1234,
                BuildingTypeId = BuildingsType.DroneFactory,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildNotExist_ThenGetNotFound_Test()
        {
            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";

            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = 1234,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"Guild with id {request.GuildId} doesn't exist.");
            response.Errors.First().FieldName.Should().Be($"GuildId");
        }

        [Test]
        public async Task AddBuildingToLand_WhenBuildingAlreadyExist_ThenGetNotFound_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.DroneFactory,
            };

            await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);
            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildBuildingAlreadyExist_ThenGetNotFound_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();
            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";

            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, token);
            ActionDataResult<AddGuildBuildingCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be("Building with type TransportBoard already exists.");
            response.Errors.First().FieldName.Should().Be("BuildingId");
        }

        [Test]
        public async Task AddBuildingToLand_WhenLandIsUndiscover_ThenGetNotFound_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.DroneFactory,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AddGuildBuilding_WhenUserIsNotOwner_ThenGetNotFound_Test()
        {
            (_, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = UserId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();
            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";

            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
            response.Errors.First().Message.Should().Be($"Only owner can add building.");
            response.Errors.First().FieldName.Should().Be($"OwnerId");
        }

        [Test]
        public async Task GetBuildingsList_WhenListExist_ThenGetThem_Test()
        {
            string endpoint = $"api/{ControllerNames.Building}/1/List";

            ActionDataResult<GetBuildingListCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetBuildingListCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Buildings.Should().HaveCount(1);
        }

        [Test]
        public async Task GetGuildBuildingsByGuildId_WhenGuildBuildingsExists_ThenGetThem_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();
            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string getEndpoint = $"api/{ControllerNames.Building}/Guild/{guild.Entity.Id}/List";


            AddGuildBuildingCommandRequest request1 = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            AddGuildBuildingCommandRequest request2 = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, request1, token);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, request2, token);

            ActionDataResult<GetGuildBuildingListCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetGuildBuildingListCommandResponse>(getEndpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.GuildBuildings.Should().HaveCount(2);
        }

        [Test]
        public async Task AddBuildingToLand_WhenUserDoesntHaveResource_ThenGetValidationErrors_Test()
        {
            (_, string newToken) = await GetNewTokenAsync();

            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Lumbermill,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, newToken);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task AddGuildBuilding_WhenGuildDoesntHaveResource_ThenGetValidationErrors_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(0),
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";

            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        public async Task UpgradeBuilding_WhenBuildingUpgraded_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Swamp,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = UserId,
                    }
                }
            });
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = UserId,
                Land = newLand.Entity,
            });
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Upgrade";

            AddBuildingToLandCommandRequest addRequest = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Lumbermill,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(addEndpoint, addRequest, Token);

            await AddAllImprovements(newLand.Entity.Id, UserId);

            UpgradeBuildingCommandRequest upgradeRequest = new()
            {
                BuildingId = addResponse.Result.Building.Id,
            };

            ActionDataResult<UpgradeBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, Token);

            upgradeResponse.IsSuccessStatusCode.Should().BeTrue();
            upgradeResponse.Result.Building.Should().NotBeNull();
            upgradeResponse.Result.Building.Level.Should().Be(2);
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenGuildBuildingUpgraded_ThenGetIt_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(1000000000),
            });
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            AddGuildBuildingCommandRequest addRequest = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, addRequest, token);

            await AddAllGuildImprovements(guild.Entity.Id);

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = addResponse.Result.GuildBuilding.Id,
            };

            ActionDataResult<UpgradeGuildBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            upgradeResponse.IsSuccessStatusCode.Should().BeTrue();
            upgradeResponse.Result.GuildBuilding.Should().NotBeNull();
            upgradeResponse.Result.GuildBuilding.Level.Should().Be(2);
        }

        [Test]
        public async Task UpgradeBuilding_WhenBuildingNotExist_ThenGetNotFound_Test()
        {
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Upgrade";

            UpgradeBuildingCommandRequest upgradeRequest = new()
            {
                BuildingId = 11232,
            };

            ActionDataResult<UpgradeBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, Token);

            upgradeResponse.IsSuccessStatusCode.Should().BeFalse();
            upgradeResponse.Result.Should().BeNull();
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenGuildBuildingNotExist_ThenGetNotFound_Test()
        {
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = 11232,
            };

            ActionDataResult<UpgradeGuildBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, Token);

            upgradeResponse.IsSuccessStatusCode.Should().BeFalse();
            upgradeResponse.Result.Should().BeNull();
            upgradeResponse.Errors.First().Message.Should().Be("Guild building with id 11232 doesn't exist.");
            upgradeResponse.Errors.First().FieldName.Should().Be("BuildingId");
        }

        public async Task UpgradeBuilding_WhenUserDoesntHaveResources_ThenGetValidationErrros_Test()
        {
            (int userId, string newToken) = await GetNewTokenAsync();
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Mountain,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = userId,
                    }
                }
            });
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = userId,
                Land = newLand.Entity,
            });
            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);
            foreach (UserResource userResource in user.UserResources)
            {
                userResource.Value += 1000;
            }
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Upgrade";

            AddBuildingToLandCommandRequest addRequest = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Slaughterhouse,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(addEndpoint, addRequest, newToken);

            UpgradeBuildingCommandRequest upgradeRequest = new()
            {
                BuildingId = addResponse.Result.Building.Id,
            };

            foreach (UserResource userResource in user.UserResources)
            {
                userResource.Value = 0;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<UpgradeBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);

            upgradeResponse.IsSuccessStatusCode.Should().BeFalse();
            upgradeResponse.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task UpgradeGuildBuilding_WhenGuildDoesntHaveResources_ThenGetValidationErrros_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            AddGuildBuildingCommandRequest addRequest = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, addRequest, token);

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = addResponse.Result.GuildBuilding.Id,
            };

            foreach (GuildResource userResource in guild.Entity.GuildResources)
            {
                userResource.Value = 0;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<UpgradeGuildBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            upgradeResponse.IsSuccessStatusCode.Should().BeFalse();
            upgradeResponse.Errors.Should().HaveCount(1);
        }

        public async Task UpgradeBuildingManyTimes_WhenBuildingUpgraded_ThenGetIt_Test()
        {
            (int userId, string newToken) = await GetNewTokenAsync();
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = userId,
                    }
                }
            });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = userId,
                Land = newLand.Entity,
            });
            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);
            user.UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000000);
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Upgrade";

            AddBuildingToLandCommandRequest addRequest = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.DroneFactory,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(addEndpoint, addRequest, newToken);

            UpgradeBuildingCommandRequest upgradeRequest = new()
            {
                BuildingId = addResponse.Result.Building.Id,
            };

            await AddAllImprovements(newLand.Entity.Id, userId);

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);
            ActionDataResult<UpgradeBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, newToken);

            upgradeResponse.IsSuccessStatusCode.Should().BeTrue();
            upgradeResponse.Result.Building.Should().NotBeNull();
            upgradeResponse.Result.Building.Level.Should().Be(7);
            upgradeResponse.Result.Building.BuildingBonuses.Should().HaveCount(1);
            Math.Round(upgradeResponse.Result.Building.BuildingBonuses.First().Bonus, 1).Should().Be(2.4);
        }

        [Test]
        public async Task UpgradeGuildBuildingManyTimes_WhenGuildBuildingUpgraded_ThenGetIt_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Guild> guild = await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                OwnerId = userId,
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(100_000_000),
            });
            await mayhemDataContext.SaveChangesAsync();

            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            AddGuildBuildingCommandRequest addRequest = new()
            {
                GuildId = guild.Entity.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> addResponse = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, addRequest, token);

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = addResponse.Result.GuildBuilding.Id,
            };

            await AddAllGuildImprovements(guild.Entity.Id);

            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);
            ActionDataResult<UpgradeGuildBuildingCommandResponse> upgradeResponse = await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            upgradeResponse.IsSuccessStatusCode.Should().BeTrue();
            upgradeResponse.Result.GuildBuilding.Should().NotBeNull();
            upgradeResponse.Result.GuildBuilding.Level.Should().Be(7);
            upgradeResponse.Result.GuildBuilding.GuildBuildingBonuses.Should().HaveCount(1);
            Math.Round(upgradeResponse.Result.GuildBuilding.GuildBuildingBonuses.First().Bonus, 1).Should().Be(3.4);
        }

        [Test]
        public async Task AddBuildingToLand_WhenBuildingHasWrongType_ThenGetValidationErrors_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Biome1,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Building}/Add";

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Lumbermill,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Errors.Should().HaveCount(1);
        }

        public async Task AddBuilding_WhenBuildingAdded_ThenIncreaseNpcAttributes_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            string endpoint = $"api/{ControllerNames.Building}/Add";

            GameUser user = await mayhemDataContext
                .GameUsers
                .SingleOrDefaultAsync(x => x.Id == userId);

            foreach (UserResource resource in user.UserResources)
            {
                resource.Value = 100000;
            }

            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = userId,
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Swamp,
                Jobs = new List<Job>()
                {
                    new Job()
                    {
                        NpcId = npc.Entity.Id,
                    }
                },
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = userId,
                    }
                }
            });
            npc.Entity.Land = newLand.Entity;
            await mayhemDataContext.SaveChangesAsync();

            List<Dal.Tables.Attribute> attributes = (await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npc.Entity.Id)).Attributes.ToList();

            double lightWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Lumbermill,
            };

            await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, token);

            lightWoodProductionBefore.Should().Be(1.5);
            heavyWoodProductionBefore.Should().Be(0.8);
            attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(1.515);
            attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.808);
        }

        public async Task UpgradeBuildingManyTime_WhenBuildingUpgraded_ThenIncreaseNpcAttributes_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            string endpoint = $"api/{ControllerNames.Building}/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Upgrade";

            GameUser user = await mayhemDataContext
                .GameUsers
                .SingleOrDefaultAsync(x => x.Id == userId);

            foreach (UserResource resource in user.UserResources)
            {
                resource.Value = 100000;
            }

            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
                UserId = userId,
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Swamp,
                Jobs = new List<Job>()
                {
                    new Job()
                    {
                        NpcId = npc.Entity.Id,
                    }
                },
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = userId,
                    }
                }
            });
            npc.Entity.Land = newLand.Entity;
            await mayhemDataContext.SaveChangesAsync();

            List<Dal.Tables.Attribute> attributes = (await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npc.Entity.Id)).Attributes.ToList();

            double lightWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            AddBuildingToLandCommandRequest request = new()
            {
                LandId = newLand.Entity.Id,
                BuildingTypeId = BuildingsType.Lumbermill,
            };

            ActionDataResult<AddBuildingToLandCommandResponse> building = await httpClientService.HttpPostAsJsonAsync<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>(endpoint, request, token);

            await AddAllImprovements(newLand.Entity.Id, userId);

            UpgradeBuildingCommandRequest upgradeRequest = new()
            {
                BuildingId = building.Result.Building.Id,
            };

            double lightWoodProductionLevel1 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel1 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel2 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel2 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel3 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel3 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel4 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel4 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel5 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel5 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel6 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel6 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await httpClientService.HttpPutAsJsonAsync<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, token);

            double lightWoodProductionLevel7 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel7 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;


            lightWoodProductionBefore.Should().Be(1.5);
            heavyWoodProductionBefore.Should().Be(0.8);
            lightWoodProductionLevel1.Should().Be(1.515);
            heavyWoodProductionLevel1.Should().Be(0.808);
            lightWoodProductionLevel2.Should().Be(1.545);
            heavyWoodProductionLevel2.Should().Be(0.824);
            lightWoodProductionLevel3.Should().Be(1.59);
            heavyWoodProductionLevel3.Should().Be(0.848);
            lightWoodProductionLevel4.Should().Be(1.6365);
            heavyWoodProductionLevel4.Should().Be(0.8728);
            lightWoodProductionLevel5.Should().Be(1.6845);
            heavyWoodProductionLevel5.Should().Be(0.8984);
            lightWoodProductionLevel6.Should().Be(1.734);
            heavyWoodProductionLevel6.Should().Be(0.9248);
            lightWoodProductionLevel7.Should().Be(1.785);
            heavyWoodProductionLevel7.Should().Be(0.952);
        }

        public async Task AddGuildBuildingAdriaCorporationHeadquarters_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == userId);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.AdriaCorporationHeadquarters).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingMechBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.MechBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingFightBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.FightBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingTransportBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.TransportBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddGuildBuildingExplorationBoard_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            foreach (Npc npc in userDb.Npcs)
            {
                List<Dal.Tables.Attribute> attributes = npc.Attributes.Where(x => AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(GuildBuildingsType.ExplorationBoard).Contains(x.AttributeTypeId)).ToList();

                foreach (Dal.Tables.Attribute attribute in attributes)
                {
                    attribute.Value.Should().BeGreaterThan(attribute.BaseValue);
                }
            }
        }

        [Test]
        public async Task AddAllGuildBuildings_WhenBuildingAdded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string endpoint = $"api/{ControllerNames.Building}/Guild/Add";
            AddGuildBuildingCommandRequest request1 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };
            AddGuildBuildingCommandRequest request2 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.MechBoard,
            };
            AddGuildBuildingCommandRequest request3 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.TransportBoard,
            };
            AddGuildBuildingCommandRequest request4 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.AdriaCorporationHeadquarters,
            };
            AddGuildBuildingCommandRequest request5 = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.ExplorationBoard,
            };

            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request1, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request2, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request3, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request4, ownerToken);
            await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(endpoint, request5, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);


            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.443);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(0.999);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1.53);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.111);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.3);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(0.9);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);

            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.554);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(0.888);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(2.04);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.111);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.4);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(0.8);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);

            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.8325);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.444);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.IronOreProduction).SingleOrDefault().Value.Should().Be(0.777);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.TitaniumProduction).SingleOrDefault().Value.Should().Be(0.3885);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatProduction).SingleOrDefault().Value.Should().Be(1.332);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealProduction).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1.02);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Healing).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MeatConsumption).SingleOrDefault().Value.Should().Be(1.2);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.CerealConsumption).SingleOrDefault().Value.Should().Be(1.0);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Discovery).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Repair).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Construction).SingleOrDefault().Value.Should().Be(1.01);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Detection).SingleOrDefault().Value.Should().Be(1.11);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.MechProduction).SingleOrDefault().Value.Should().Be(1.06);
        }

        [Test]
        public async Task UpgradeGuildBuildingFightBoard_WhenBuildingUpgraded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            AddGuildBuildingCommandRequest addRequest = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> addedBuilding = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, addRequest, ownerToken);

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = addedBuilding.Result.GuildBuilding.Id,
            };

            await AddAllGuildImprovements(guildDb.Id);

            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.545);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(2.06);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.03);
        }

        [Test]
        public async Task UpgradeGuildBuildingFightBoardManyTime_WhenBuildingUpgraded_ThenChangeAttributesFotEachNpc_Test()
        {
            (int ownerId, string ownerToken) = await GetNewTokenAsync();
            (int userId, string userToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.Where(x => x.Id == userId).SingleOrDefaultAsync();
            user.Npcs = new List<Npc>()
            {
                new Npc()
                {
                    NpcTypeId = NpcsType.Scout,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Scout),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Soldier,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Soldier),
                },
                new Npc()
                {
                    NpcTypeId = NpcsType.Mechanic,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Mechanic),
                },
            };
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<CreateGuildCommandResponse> guild = await CreateGuildAsync(Guid.NewGuid().ToString(), ownerToken);
            Guild guildDb = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guild.Result.Guild.Id);
            foreach (GuildResource res in guildDb.GuildResources)
            {
                res.Value = 10000000;
            }
            await mayhemDataContext.SaveChangesAsync();

            ActionDataResult<AskToJoinGuildByUserCommandResponse> invitation = await AskToJoinGuildByUserAsync(guild.Result.Guild.Id, userToken);
            await AcceptInvitationByOwnerAsync(invitation.Result.InviteUser.Invitation.Id, ownerToken);

            string addEndpoint = $"api/{ControllerNames.Building}/Guild/Add";
            string upgradeEndpoint = $"api/{ControllerNames.Building}/Guild/Upgrade";

            AddGuildBuildingCommandRequest addRequest = new()
            {
                GuildId = guild.Result.Guild.Id,
                GuildBuildingTypeId = GuildBuildingsType.FightBoard,
            };

            ActionDataResult<AddGuildBuildingCommandResponse> addedBuilding = await httpClientService.HttpPostAsJsonAsync<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>(addEndpoint, addRequest, ownerToken);

            UpgradeGuildBuildingCommandRequest upgradeRequest = new()
            {
                GuildBuildingId = addedBuilding.Result.GuildBuilding.Id,
            };

            await AddAllGuildImprovements(guildDb.Id);

            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, ownerToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, ownerToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, ownerToken);
            await httpClientService.HttpPutAsJsonAsync<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>(upgradeEndpoint, upgradeRequest, ownerToken);

            GameUser userDb = await mayhemDataContext
                .GameUsers
                .Include(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            userDb.Npcs.ToList()[0].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.593);
            userDb.Npcs.ToList()[1].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(2.124);
            userDb.Npcs.ToList()[2].Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).First().Value.Should().Be(1.062);
        }

        private async Task AddAllImprovements(long landId, int userId)
        {
            foreach (ImprovementsType item in Enum.GetValues<ImprovementsType>().Cast<ImprovementsType>())
            {
                await improvementRepository.AddImprovementAsync(new Dal.Dto.Dtos.ImprovementDto()
                {
                    ImprovementTypeId = item,
                    LandId = landId,
                }, userId);
            }
        }

        private async Task AddAllGuildImprovements(int guildId)
        {
            foreach (GuildImprovementsType item in Enum.GetValues<GuildImprovementsType>().Cast<GuildImprovementsType>())
            {
                await improvementRepository.AddGuildImprovementAsync(new Dal.Dto.Dtos.GuildImprovementDto()
                {
                    GuildId = guildId,
                    GuildImprovementTypeId = item,
                });
            }
        }
    }
}
