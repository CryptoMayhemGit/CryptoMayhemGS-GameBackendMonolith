using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhen.Bl.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Services.Implementations
{
    public class ImprovementValidationService : IImprovementValidationService
    {
        public bool ValidateImprovement(int level, BuildingsType buildingsType, IEnumerable<ImprovementsType> improvements)
        {
            if (level == 1)
            {
                return true;
            }

            return buildingsType switch
            {
                BuildingsType.Lumbermill => level switch
                {
                    2 => ImprovementValidationDictionary.Level2Lumbermill.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3Lumbermill.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2Lumbermill.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3Lumbermill.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.OreMine => level switch
                {
                    2 => ImprovementValidationDictionary.Level2OreMine.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3OreMine.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2OreMine.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3OreMine.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.MechanicalWorkshop => level switch
                {
                    2 => ImprovementValidationDictionary.Level2MechanicalWorkshop.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3MechanicalWorkshop.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2MechanicalWorkshop.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3MechanicalWorkshop.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.DroneFactory => level switch
                {
                    2 => ImprovementValidationDictionary.Level2DroneFactory.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3DroneFactory.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2DroneFactory.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3DroneFactory.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.CombatWorkshop => level switch
                {
                    2 => ImprovementValidationDictionary.Level2CombatWorkshop.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3CombatWorkshop.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2CombatWorkshop.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3CombatWorkshop.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.Farm => level switch
                {
                    2 => ImprovementValidationDictionary.Level2Farm.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3Farm.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2Farm.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3Farm.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.Slaughterhouse => level switch
                {
                    2 => ImprovementValidationDictionary.Level2Slaughterhouse.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3Slaughterhouse.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2Slaughterhouse.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3Slaughterhouse.All(improvements.Contains),
                    _ => false,
                },
                BuildingsType.Guardhouse => level switch
                {
                    2 => ImprovementValidationDictionary.Level2Guardhouse.All(improvements.Contains),
                    3 => ImprovementValidationDictionary.Level3Guardhouse.All(improvements.Contains),
                    int l when l > 3 =>
                    ImprovementValidationDictionary.Level2Guardhouse.All(improvements.Contains)
                    && ImprovementValidationDictionary.Level3Guardhouse.All(improvements.Contains),
                    _ => false,
                },
                _ => false,
            };
        }
    }
}
