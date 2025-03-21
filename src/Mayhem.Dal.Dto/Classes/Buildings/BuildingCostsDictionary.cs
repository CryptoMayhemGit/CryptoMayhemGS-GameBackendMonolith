using Mayhem.Dal.Dto.Enums.Dictionaries;
using System;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Buildings
{
    public static class BuildingCostsDictionary
    {
        public static Dictionary<ResourcesType, int> GetBuildingCosts(BuildingsType type, int level)
        {
            return type switch
            {
                BuildingsType.Lumbermill => level switch
                {
                    1 => Level1Lumbermill,
                    2 => Level2Lumbermill,
                    3 => Level3Lumbermill,
                    int l when l > 3 => GetLumbermillByPattern(level),
                    _ => null
                },
                BuildingsType.OreMine => level switch
                {
                    1 => Level1OreMine,
                    2 => Level2OreMine,
                    3 => Level3OreMine,
                    int l when l > 3 => GetOreMineByPattern(level),
                    int => null,
                },
                BuildingsType.MechanicalWorkshop => level switch
                {
                    1 => Level1Workshop,
                    2 => Level2Workshop,
                    3 => Level3Workshop,
                    int l when l > 3 => GetMechanicalWorkshopByPattern(level),
                    _ => null,
                },
                BuildingsType.DroneFactory => level switch
                {
                    1 => Level1DroneFactory,
                    2 => Level2DroneFactory,
                    3 => Level3DroneFactory,
                    int l when l > 3 => GetDroneFactoryByPattern(level),
                    _ => null,
                },
                BuildingsType.CombatWorkshop => level switch
                {
                    1 => Level1CombatWorkshop,
                    2 => Level2CombatWorkshop,
                    3 => Level3CombatWorkshop,
                    int l when l > 3 => GetCombatWorkshopByPattern(level),
                    _ => null,
                },
                BuildingsType.Farm => level switch
                {
                    1 => Level1Farm,
                    2 => Level2Farm,
                    3 => Level3Farm,
                    int l when l > 3 => GetFarmByPattern(level),
                    _ => null,
                },
                BuildingsType.Slaughterhouse => level switch
                {
                    1 => Level1Slaughterhouse,
                    2 => Level2Slaughterhouse,
                    3 => Level3Slaughterhouse,
                    int l when l > 3 => GetSlaughterhouseByPattern(level),
                    _ => null,
                },
                BuildingsType.Guardhouse => level switch
                {
                    1 => Level1Guardhouse,
                    2 => Level2Guardhouse,
                    3 => Level3Guardhouse,
                    int l when l > 3 => GetGuardhouseByPattern(level),
                    _ => null,
                },

                _ => null,
            };
        }

        private static Dictionary<ResourcesType, int> Level1Lumbermill => new()
        {
            { ResourcesType.LightWood, 10 },
            { ResourcesType.HeavyWood, 5 },
            { ResourcesType.IronOre, 4 },
        };

        private static Dictionary<ResourcesType, int> Level2Lumbermill => new()
        {
            { ResourcesType.LightWood, 37 },
            { ResourcesType.TitaniumOre, 18 },
            { ResourcesType.IronOre, 27 },
            { ResourcesType.Mechanium, 12 },
        };

        private static Dictionary<ResourcesType, int> Level3Lumbermill => new()
        {
            { ResourcesType.LightWood, 90 },
            { ResourcesType.TitaniumOre, 59 },
            { ResourcesType.IronOre, 77 },
            { ResourcesType.Mechanium, 45 },
        };

        private static Dictionary<ResourcesType, int> Level1OreMine => new()
        {
            { ResourcesType.LightWood, 7 },
            { ResourcesType.HeavyWood, 10 },
            { ResourcesType.IronOre, 7 },
        };

        private static Dictionary<ResourcesType, int> Level2OreMine => new()
        {
            { ResourcesType.LightWood, 24 },
            { ResourcesType.TitaniumOre, 33 },
            { ResourcesType.IronOre, 21 },
            { ResourcesType.Mechanium, 13 },
        };

        private static Dictionary<ResourcesType, int> Level3OreMine => new()
        {
            { ResourcesType.LightWood, 67 },
            { ResourcesType.TitaniumOre, 75 },
            { ResourcesType.IronOre, 76 },
            { ResourcesType.Mechanium, 53 },
        };

        private static Dictionary<ResourcesType, int> Level1Workshop => new()
        {
            { ResourcesType.LightWood, 2 },
            { ResourcesType.HeavyWood, 15 },
            { ResourcesType.IronOre, 10 },
        };

        private static Dictionary<ResourcesType, int> Level2Workshop => new()
        {
            { ResourcesType.LightWood, 24 },
            { ResourcesType.HeavyWood, 37 },
            { ResourcesType.TitaniumOre, 25 },
            { ResourcesType.Mechanium, 10 },
        };

        private static Dictionary<ResourcesType, int> Level3Workshop => new()
        {
            { ResourcesType.LightWood, 79 },
            { ResourcesType.HeavyWood, 83 },
            { ResourcesType.TitaniumOre, 74 },
            { ResourcesType.Mechanium, 60 },
        };

        private static Dictionary<ResourcesType, int> Level1DroneFactory => new()
        {
            { ResourcesType.IronOre, 20 },
            { ResourcesType.TitaniumOre, 20 },
        };

        private static Dictionary<ResourcesType, int> Level2DroneFactory => new()
        {
            { ResourcesType.IronOre, 46 },
            { ResourcesType.TitaniumOre, 36 },
            { ResourcesType.Mechanium, 22 },
        };

        private static Dictionary<ResourcesType, int> Level3DroneFactory => new()
        {
            { ResourcesType.IronOre, 101 },
            { ResourcesType.TitaniumOre, 95 },
            { ResourcesType.Mechanium, 65 },
        };

        private static Dictionary<ResourcesType, int> Level1CombatWorkshop => new()
        {
            { ResourcesType.IronOre, 10 },
            { ResourcesType.HeavyWood, 10 },
            { ResourcesType.TitaniumOre, 3 },
        };

        private static Dictionary<ResourcesType, int> Level2CombatWorkshop => new()
        {
            { ResourcesType.IronOre, 39 },
            { ResourcesType.HeavyWood, 34 },
            { ResourcesType.TitaniumOre, 27 },
            { ResourcesType.Mechanium, 12 },
        };

        private static Dictionary<ResourcesType, int> Level3CombatWorkshop => new()
        {
            { ResourcesType.IronOre, 87 },
            { ResourcesType.HeavyWood, 90 },
            { ResourcesType.TitaniumOre, 75 },
            { ResourcesType.Mechanium, 59 },
        };

        private static Dictionary<ResourcesType, int> Level1Farm => new()
        {
            { ResourcesType.LightWood, 12 },
            { ResourcesType.HeavyWood, 4 },
        };

        private static Dictionary<ResourcesType, int> Level2Farm => new()
        {
            { ResourcesType.Cereal, 42 },
            { ResourcesType.LightWood, 53 },
            { ResourcesType.Meat, 27 },
            { ResourcesType.Mechanium, 13 },
        };

        private static Dictionary<ResourcesType, int> Level3Farm => new()
        {
            { ResourcesType.Cereal, 95 },
            { ResourcesType.LightWood, 107 },
            { ResourcesType.Meat, 80 },
            { ResourcesType.Mechanium, 60 },
        };

        private static Dictionary<ResourcesType, int> Level1Slaughterhouse => new()
        {
            { ResourcesType.LightWood, 7 },
            { ResourcesType.HeavyWood, 9 },
        };

        private static Dictionary<ResourcesType, int> Level2Slaughterhouse => new()
        {
            { ResourcesType.Cereal, 35 },
            { ResourcesType.LightWood, 29 },
            { ResourcesType.HeavyWood, 29 },
            { ResourcesType.Mechanium, 12 },
        };

        private static Dictionary<ResourcesType, int> Level3Slaughterhouse => new()
        {
            { ResourcesType.Cereal, 82 },
            { ResourcesType.LightWood, 77 },
            { ResourcesType.HeavyWood, 84 },
            { ResourcesType.Mechanium, 66 },
        };

        private static Dictionary<ResourcesType, int> Level1Guardhouse => new()
        {
            { ResourcesType.HeavyWood, 11 },
            { ResourcesType.IronOre, 6 },
        };

        private static Dictionary<ResourcesType, int> Level2Guardhouse => new()
        {
            { ResourcesType.HeavyWood, 27 },
            { ResourcesType.TitaniumOre, 26 },
            { ResourcesType.IronOre, 39 },
            { ResourcesType.Mechanium, 16 },
        };

        private static Dictionary<ResourcesType, int> Level3Guardhouse => new()
        {
            { ResourcesType.HeavyWood, 82 },
            { ResourcesType.TitaniumOre, 72 },
            { ResourcesType.IronOre, 81 },
            { ResourcesType.Mechanium, 69 },
        };

        private static Dictionary<ResourcesType, int> GetLumbermillByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.LightWood, getLightWoodCost(level)},
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.IronOre, getIronOreCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getLightWoodCost(int level)
            {
                return (int)Math.Ceiling(90 * level * 1.2 + Math.Pow(level, 3.2));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(59 * level * 1.2 + Math.Pow(level, 3));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(77 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(45 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetOreMineByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.LightWood, getLightWoodCost(level)},
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.IronOre, getIronOreCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getLightWoodCost(int level)
            {
                return (int)Math.Ceiling(67 * level * 1.2 + Math.Pow(level, 3.2));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(75 * level * 1.2 + Math.Pow(level, 3));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(76 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(53 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetMechanicalWorkshopByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.LightWood, getLightWoodCost(level)},
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.HeavyWood, getHeavyWoodCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getLightWoodCost(int level)
            {
                return (int)Math.Ceiling(79 * level * 1.2 + Math.Pow(level, 3.2));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(74 * level * 1.2 + Math.Pow(level, 3));
            }

            int getHeavyWoodCost(int level)
            {
                return (int)Math.Ceiling(83 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(60 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetDroneFactoryByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.IronOre, getIronOreCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(101 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(95 * level * 1.2 + Math.Pow(level, 3));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(65 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetCombatWorkshopByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.IronOre, getIronOreCost(level)},
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.HeavyWood, getHeavyWoodCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(89 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(75 * level * 1.2 + Math.Pow(level, 3));
            }

            int getHeavyWoodCost(int level)
            {
                return (int)Math.Ceiling(90 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(59 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetFarmByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.Cereal, getCerealCost(level)},
                { ResourcesType.LightWood, getLightWoodCost(level)},
                { ResourcesType.Meat, getMeatCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getCerealCost(int level)
            {
                return (int)Math.Ceiling(95 * level * 1.2 + Math.Pow(level, 2.85));
            }

            int getLightWoodCost(int level)
            {
                return (int)Math.Ceiling(107 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMeatCost(int level)
            {
                return (int)Math.Ceiling(80 * level * 1.2 + Math.Pow(level, 2.78));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(60 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetSlaughterhouseByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.Cereal, getCerealCost(level)},
                { ResourcesType.LightWood, getLightWoodCost(level)},
                { ResourcesType.HeavyWood, getHeavyWoodCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getCerealCost(int level)
            {
                return (int)Math.Ceiling(82 * level * 1.2 + Math.Pow(level, 2.85));
            }

            int getLightWoodCost(int level)
            {
                return (int)Math.Ceiling(77 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getHeavyWoodCost(int level)
            {
                return (int)Math.Ceiling(84 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(66 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetGuardhouseByPattern(int level)
        {
            return new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.HeavyWood, getHeavyWoodCost(level)},
                { ResourcesType.TitaniumOre, getTitaniumCost(level)},
                { ResourcesType.IronOre, getIronOreCost(level)},
                { ResourcesType.Mechanium, getMechaniumCost(level)},
            };

            int getHeavyWoodCost(int level)
            {
                return (int)Math.Ceiling(82 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(72 * level * 1.2 + Math.Pow(level, 3));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(81 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(69 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }
    }
}
