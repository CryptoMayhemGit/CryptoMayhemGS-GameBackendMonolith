using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using Mayhem.Package.Bl.Interfaces;

namespace Mayhem.Package.Bl.Services
{
    public class NpcGeneratorService : INpcGeneratorService
    {
        private readonly GeneratorConfiguration generatorConfiguration;
        private readonly IAttributeService attributeService;

        public NpcGeneratorService(
            IMayhemConfigurationService mayhemConfigurationService,
            IAttributeService attributeService)
        {
            this.attributeService = attributeService;

            generatorConfiguration = mayhemConfigurationService.MayhemConfiguration.GeneratorConfiguration;
        }

        public IEnumerable<NpcDto> GeneratNpcs()
        {
            List<NpcDto> npcs = new();
            List<NpcDto> avatars = new();

            foreach (NpcsType type in Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>())
            {
                for (int creationCounter = 1; creationCounter <= generatorConfiguration.NftNpcCreations; creationCounter++)
                {
                    for (int amount = 0; amount < GetNftAmount(type, creationCounter); amount++)
                    {
                        NpcDto itemDto = new()
                        {
                            NpcTypeId = type,
                            IsAvatar = false,
                            Address = $"/Npcs/{type}{creationCounter}.jpg",
                            Attributes = attributeService.GetBasicNpcAttributesWithValue(type),
                            NpcStatusId = NpcsStatus.None,
                            NpcHealthStateId = NpcHealthsState.Healthy,
                        };
                        npcs.Add(itemDto);
                    }
                }
            }

            foreach (NpcsType type in Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>())
            {
                for (int creation = 1; creation <= generatorConfiguration.NftAvatarAmount; creation++)
                {
                    NpcDto itemDto = new()
                    {
                        NpcTypeId = type,
                        IsAvatar = true,
                        Address = $"/Avatars/{type}{creation}.jpg",
                        Attributes = attributeService.GetBasicNpcAttributesWithValue(type), 
                        NpcStatusId = NpcsStatus.None,
                        NpcHealthStateId = NpcHealthsState.Healthy,
                    };
                    avatars.Add(itemDto);
                }
            }

            int nameCounter = 0;

            foreach (NpcDto itemDto in npcs)
            {
                itemDto.Name = $"Npc {++nameCounter}";
            }

            nameCounter = 0;
            foreach (NpcDto itemDto in avatars)
            {
                itemDto.Name = $"Avatar {++nameCounter}";
            }

            return npcs.Union(avatars);
        }

        private static int GetNftAmount(NpcsType type, int nftNpcCreations)
        {
            return type switch
            {
                NpcsType.Miner => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Lumberjack => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Hunter => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Farmer => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Soldier => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Scout => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Mechanic => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Engineer => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Biologist => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Chemist => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Pilot => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 56,
                    6 => 56,
                    _ => 0,
                },
                NpcsType.Doctor => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Geologist => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 57,
                    6 => 57,
                    _ => 0,
                },
                NpcsType.Fitter => nftNpcCreations switch
                {
                    1 => 57,
                    2 => 57,
                    3 => 57,
                    4 => 57,
                    5 => 56,
                    6 => 56,
                    _ => 0,
                },
                _ => throw new Exception("Out of range"),
            };
        }
    }
}
