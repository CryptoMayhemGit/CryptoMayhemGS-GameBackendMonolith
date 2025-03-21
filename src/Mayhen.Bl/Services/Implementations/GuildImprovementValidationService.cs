using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhen.Bl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Services.Implementations
{
    public class GuildImprovementValidationService : IGuildImprovementValidationService
    {
        public bool ValidateImprovement(int level, GuildBuildingsType guildBuildingsType, IEnumerable<GuildImprovementsType> guildImprovements)
        {
            if (level == 1)
            {
                return true;
            }

            return guildBuildingsType switch
            {
                GuildBuildingsType.AdriaCorporationHeadquarters => level switch
                {
                    2 => GuildImprovementValidationDictionary.Level2AdriaCorporationHeadquarters.All(guildImprovements.Contains),
                    3 => GuildImprovementValidationDictionary.Level3AdriaCorporationHeadquarters.All(guildImprovements.Contains),
                    int l when l > 3 =>
                    GuildImprovementValidationDictionary.Level2AdriaCorporationHeadquarters.All(guildImprovements.Contains)
                    && GuildImprovementValidationDictionary.Level3AdriaCorporationHeadquarters.All(guildImprovements.Contains),
                    _ => false,
                },
                GuildBuildingsType.ExplorationBoard => level switch
                {
                    2 => GuildImprovementValidationDictionary.Level2ExplorationBoard.All(guildImprovements.Contains),
                    3 => GuildImprovementValidationDictionary.Level3ExplorationBoard.All(guildImprovements.Contains),
                    int l when l > 3 =>
                    GuildImprovementValidationDictionary.Level2ExplorationBoard.All(guildImprovements.Contains)
                    && GuildImprovementValidationDictionary.Level3ExplorationBoard.All(guildImprovements.Contains),
                    _ => false,
                },
                GuildBuildingsType.MechBoard => level switch
                {
                    2 => GuildImprovementValidationDictionary.Level2MechManagement.All(guildImprovements.Contains),
                    3 => GuildImprovementValidationDictionary.Level3MechManagement.All(guildImprovements.Contains),
                    int l when l > 3 =>
                    GuildImprovementValidationDictionary.Level2MechManagement.All(guildImprovements.Contains)
                    && GuildImprovementValidationDictionary.Level3MechManagement.All(guildImprovements.Contains),
                    _ => false,
                },
                GuildBuildingsType.FightBoard => level switch
                {
                    2 => GuildImprovementValidationDictionary.Level2FightBoard.All(guildImprovements.Contains),
                    3 => GuildImprovementValidationDictionary.Level3FightBoard.All(guildImprovements.Contains),
                    int l when l > 3 =>
                    GuildImprovementValidationDictionary.Level2FightBoard.All(guildImprovements.Contains)
                    && GuildImprovementValidationDictionary.Level3FightBoard.All(guildImprovements.Contains),
                    _ => false,
                },
                GuildBuildingsType.TransportBoard => level switch
                {
                    2 => GuildImprovementValidationDictionary.Level2TransportAuthority.All(guildImprovements.Contains),
                    3 => GuildImprovementValidationDictionary.Level3TransportAuthority.All(guildImprovements.Contains),
                    int l when l > 3 =>
                    GuildImprovementValidationDictionary.Level2TransportAuthority.All(guildImprovements.Contains)
                    && GuildImprovementValidationDictionary.Level3TransportAuthority.All(guildImprovements.Contains),
                    _ => false,
                },
                _ => false,
            };
        }
    }
}
