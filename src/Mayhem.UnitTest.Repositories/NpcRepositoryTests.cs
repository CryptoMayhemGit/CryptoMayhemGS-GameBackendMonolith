using AutoMapper;
using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class NpcRepositoryTests : UnitTestBase
    {
        private INpcRepository npcRepository;
        private IMayhemDataContext mayhemDataContext;
        private IMapper mapper;
        private IDamageService damageService;

        [OneTimeSetUp]
        public void SetUp()
        {
            npcRepository = GetService<INpcRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            mapper = GetService<IMapper>();
            damageService = GetService<IDamageService>();
        }

        [Test]
        public async Task GetNpcByNftId_WhenNpcExists_ThenGetIt_Test()
        {
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc());
            await mayhemDataContext.SaveChangesAsync();

            NpcDto npc = await npcRepository.GetNpcByNftIdAsync(newNpc.Entity.Id);

            npc.Should().NotBeNull();
        }

        [Test]
        public async Task GetNpcByNftId_WhenNpcNotExists_ThenGetNull_Test()
        {
            NpcDto npc = await npcRepository.GetNpcByNftIdAsync(1432);

            npc.Should().BeNull();
        }

        [Test]
        public async Task GetAvailableNpcs_WhenNpcsNotExists_ThenGetEmpty_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<NpcDto> npcs = await npcRepository.GetAvailableNpcsByUserIdAsync(user.Entity.Id);

            npcs.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateNpcHealthWithAttributes_WhenNpcsUpdated_ThenChangeHealthStateAndAttributesValues_Test()
        {
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(
                new Npc()
                {
                    NpcHealthStateId = NpcHealthsState.Healthy,
                    NpcTypeId = NpcsType.Biologist,
                    Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Biologist),
                }
            );

            await mayhemDataContext.SaveChangesAsync();

            List<AttributeDto> newAttributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Biologist).Select(x => mapper.Map<AttributeDto>(x)).ToList();
            foreach (AttributesType item in damageService.AttributesToEdit)
            {
                newAttributes.Where(x => x.AttributeTypeId == item).Single().Value /= 2;
            }

            bool result = await npcRepository.UpdateNpcHealthWithAttributesAsync(npc.Entity.Id, newAttributes, NpcHealthsState.Dying);

            Npc npcDb = await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npc.Entity.Id);

            result.Should().BeTrue();
            npcDb.NpcHealthStateId.Should().Be(NpcHealthsState.Dying);

            foreach (AttributesType item in damageService.AttributesToEdit)
            {
                npcDb.Attributes.Where(x => x.AttributeTypeId == item).Select(x => x.Value).SingleOrDefault()
                .Should().BeLessThan(npcDb.Attributes.Where(x => x.AttributeTypeId == item).Select(x => x.BaseValue).Single());
            }
        }

        [Test]
        public async Task GetAvailableNpcs_WhenNpcsExists_ThenGetThem_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                Npcs = new List<Npc>()
                {
                    new Npc()
                    {
                        Building = new Building(),
                    },
                    new Npc()
                    {
                    },
                    new Npc()
                    {
                    },
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<NpcDto> npcs = await npcRepository.GetAvailableNpcsByUserIdAsync(user.Entity.Id);

            npcs.Should().HaveCount(2);
        }
    }
}
