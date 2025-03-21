using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Messages;
using System;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.GuildBuildings
{
    public static class GuildBuildingCostsDictionary
    {
        public static Dictionary<ResourcesType, int> GetGuildBuildingCosts(GuildBuildingsType type, int level)
        {
            return type switch
            {
                GuildBuildingsType.AdriaCorporationHeadquarters => level switch
                {
                    1 => Level1AdriaCorporationHeadquarters,
                    2 => Level2AdriaCorporationHeadquarters,
                    3 => Level3AdriaCorporationHeadquarters,
                    int l when l > 3 => GetAdriaCorporationHeadquartersByPattern(level),
                    _ => null
                },
                GuildBuildingsType.ExplorationBoard => level switch
                {
                    1 => Level1ExplorationBoard,
                    2 => Level2ExplorationBoard,
                    3 => Level3ExplorationBoard,
                    int l when l > 3 => GetExplorationBoardByPattern(level),
                    _ => null
                },
                GuildBuildingsType.MechBoard => level switch
                {
                    1 => Level1MechManagement,
                    2 => Level2MechManagement,
                    3 => Level3MechManagement,
                    int l when l > 3 => GetMechManagementByPattern(level),
                    _ => null
                },
                GuildBuildingsType.FightBoard => level switch
                {
                    1 => Level1FightBoard,
                    2 => Level2FightBoard,
                    3 => Level3FightBoard,
                    int l when l > 3 => GetFightBoardByPattern(level),
                    _ => null
                },
                GuildBuildingsType.TransportBoard => level switch
                {
                    1 => Level1TransportAuthority,
                    2 => Level2TransportAuthority,
                    3 => Level3TransportAuthority,
                    int l when l > 3 => GetTransportAuthorityByPattern(level),
                    _ => null
                },
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(GuildBuildingsType)),
            };
        }

        private static Dictionary<ResourcesType, int> Level1AdriaCorporationHeadquarters => new()
        {
            { ResourcesType.HeavyWood, 40000 },
            { ResourcesType.TitaniumOre, 25000 },
            { ResourcesType.IronOre, 38000 },
            { ResourcesType.Mechanium, 16000 },
        };

        private static Dictionary<ResourcesType, int> Level2AdriaCorporationHeadquarters => new()
        {
            { ResourcesType.HeavyWood, 84000 },
            { ResourcesType.TitaniumOre, 61000 },
            { ResourcesType.IronOre, 69000 },
            { ResourcesType.Mechanium, 42000 },
        };

        private static Dictionary<ResourcesType, int> Level3AdriaCorporationHeadquarters => new()
        {
            { ResourcesType.HeavyWood, 250000 },
            { ResourcesType.TitaniumOre, 195000 },
            { ResourcesType.IronOre, 230000 },
            { ResourcesType.Mechanium, 155000 },
        };

        private static Dictionary<ResourcesType, int> Level1ExplorationBoard => new()
        {
            { ResourcesType.HeavyWood, 35000 },
            { ResourcesType.TitaniumOre, 18000 },
            { ResourcesType.IronOre, 23000 },
            { ResourcesType.Mechanium, 8000 },
        };

        private static Dictionary<ResourcesType, int> Level2ExplorationBoard => new()
        {
            { ResourcesType.HeavyWood, 70000 },
            { ResourcesType.TitaniumOre, 47000 },
            { ResourcesType.IronOre, 53000 },
            { ResourcesType.Mechanium, 35000 },
        };

        private static Dictionary<ResourcesType, int> Level3ExplorationBoard => new()
        {
            { ResourcesType.HeavyWood, 220000 },
            { ResourcesType.TitaniumOre, 150000 },
            { ResourcesType.IronOre, 170000 },
            { ResourcesType.Mechanium, 130000 },
        };

        private static Dictionary<ResourcesType, int> Level1MechManagement => new()
        {
            { ResourcesType.HeavyWood, 27000 },
            { ResourcesType.TitaniumOre, 16000 },
            { ResourcesType.IronOre, 15000 },
            { ResourcesType.Mechanium, 6000 },
        };

        private static Dictionary<ResourcesType, int> Level2MechManagement => new()
        {
            { ResourcesType.HeavyWood, 65000 },
            { ResourcesType.TitaniumOre, 45000 },
            { ResourcesType.IronOre, 44000 },
            { ResourcesType.Mechanium, 32000 },
        };

        private static Dictionary<ResourcesType, int> Level3MechManagement => new()
        {
            { ResourcesType.HeavyWood, 190000 },
            { ResourcesType.TitaniumOre, 175000 },
            { ResourcesType.IronOre, 155000 },
            { ResourcesType.Mechanium, 140000 },
        };

        private static Dictionary<ResourcesType, int> Level1FightBoard => new()
        {
            { ResourcesType.HeavyWood, 32000 },
            { ResourcesType.TitaniumOre, 19000 },
            { ResourcesType.IronOre, 22000 },
            { ResourcesType.Mechanium, 8000 },
        };

        private static Dictionary<ResourcesType, int> Level2FightBoard => new()
        {
            { ResourcesType.HeavyWood, 71000 },
            { ResourcesType.TitaniumOre, 57000 },
            { ResourcesType.IronOre, 48000 },
            { ResourcesType.Mechanium, 38000 },
        };

        private static Dictionary<ResourcesType, int> Level3FightBoard => new()
        {
            { ResourcesType.HeavyWood, 200000 },
            { ResourcesType.TitaniumOre, 175000 },
            { ResourcesType.IronOre, 145000 },
            { ResourcesType.Mechanium, 150000 },
        };

        private static Dictionary<ResourcesType, int> Level1TransportAuthority => new()
        {
            { ResourcesType.HeavyWood, 18000 },
            { ResourcesType.TitaniumOre, 10000 },
            { ResourcesType.IronOre, 12000 },
            { ResourcesType.Mechanium, 4000 },
        };

        private static Dictionary<ResourcesType, int> Level2TransportAuthority => new()
        {
            { ResourcesType.HeavyWood, 44000 },
            { ResourcesType.TitaniumOre, 46000 },
            { ResourcesType.IronOre, 36000 },
            { ResourcesType.Mechanium, 29000 },
        };

        private static Dictionary<ResourcesType, int> Level3TransportAuthority => new()
        {
            { ResourcesType.HeavyWood, 165000 },
            { ResourcesType.TitaniumOre, 210000 },
            { ResourcesType.IronOre, 160000 },
            { ResourcesType.Mechanium, 125000 },
        };

        private static Dictionary<ResourcesType, int> GetAdriaCorporationHeadquartersByPattern(int level)
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
                return (int)Math.Ceiling(250000 * level * 1.2 + Math.Pow(level, 3));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(195000 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(230000 * level * 1.2 + Math.Pow(level, 2.9));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(155000 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetExplorationBoardByPattern(int level)
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
                return (int)Math.Ceiling(220000 * level * 1.2 + Math.Pow(level, 3));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(150000 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(170000 * level * 1.2 + Math.Pow(level, 2.9));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(130000 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetMechManagementByPattern(int level)
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
                return (int)Math.Ceiling(190000 * level * 1.2 + Math.Pow(level, 3));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(175000 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(155000 * level * 1.2 + Math.Pow(level, 2.9));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(140000 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetFightBoardByPattern(int level)
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
                return (int)Math.Ceiling(200000 * level * 1.2 + Math.Pow(level, 3));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(175000 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(145000 * level * 1.2 + Math.Pow(level, 2.9));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(150000 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }

        private static Dictionary<ResourcesType, int> GetTransportAuthorityByPattern(int level)
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
                return (int)Math.Ceiling(165000 * level * 1.2 + Math.Pow(level, 3));
            }

            int getTitaniumCost(int level)
            {
                return (int)Math.Ceiling(210000 * level * 1.2 + Math.Pow(level, 3.05));
            }

            int getIronOreCost(int level)
            {
                return (int)Math.Ceiling(160000 * level * 1.2 + Math.Pow(level, 2.9));
            }

            int getMechaniumCost(int level)
            {
                return (int)Math.Ceiling(125000 * level * 1.2 + Math.Pow(level, 2.9));
            }
        }
    }
}
