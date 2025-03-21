using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using Mayhem.Package.Bl.Interfaces;

namespace Mayhem.Package.Bl.Services
{
    public class ItemGeneratorService : IItemGeneratorService
    {
        public IEnumerable<ItemDto> GenerateItems()
        {
            List<ItemDto> items = new();

            foreach (ItemsType type in Enum.GetValues(typeof(ItemsType)).Cast<ItemsType>())
            {
                for (int amount = 0; amount < GetNftItemAmount(type); amount++)
                {
                    ItemDto itemDto = new()
                    {
                        ItemTypeId = type,
                        Address = $"/Items/{type}.jpg",
                        ItemBonuses = GenerateItemBonus(type)
                    };
                    items.Add(itemDto);
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Name = $"Item {i}";
            }

            return items;
        }

        private static int GetNftItemAmount(ItemsType type) => type switch
        {
            ItemsType.AirHammer => 180,
            ItemsType.GravityHammer => 180,
            ItemsType.SonicHammer => 180,
            ItemsType.Excavator => 180,
            ItemsType.Chainsaw => 180,
            ItemsType.Gravitysaw => 180,
            ItemsType.LaserCutter => 180,
            ItemsType.Harvester => 180,
            ItemsType.HuntingRifle => 180,
            ItemsType.RapidFireRifle => 180,
            ItemsType.SniperRifle => 180,
            ItemsType.OffRoadVehicleWithMachineGun => 180,
            ItemsType.SeedlingContainer => 180,
            ItemsType.SeedlingKit => 180,
            ItemsType.HydroponicVessel => 180,
            ItemsType.HydroponicContainer => 180,
            ItemsType.AutomaticCarbine => 180,
            ItemsType.LargeCaliberRifle => 180,
            ItemsType.LaserRifle => 180,
            ItemsType.ArmoredCarWithMachineGun => 180,
            ItemsType.Binoculars => 180,
            ItemsType.TacticalScope => 180,
            ItemsType.NightVision => 180,
            ItemsType.Motorcycle => 180,
            ItemsType.MultiTool => 177,
            ItemsType.ToolBox => 177,
            ItemsType.Drill => 176,
            ItemsType.ContainerWithWorkshop => 176,
            ItemsType.HammerAndScrewdriver => 178,
            ItemsType.ElectronicMeter => 178,
            ItemsType.MultiFunctionRobot => 177,
            ItemsType.CraneOnTheCar => 177,
            ItemsType.HandProbe => 178,
            ItemsType.LaserProbe => 178,
            ItemsType.InspectionCrate => 177,
            ItemsType.TestContainer => 177,
            ItemsType.SetOfMeasuringCup => 178,
            ItemsType.Distiller => 178,
            ItemsType.DiagnosticStation => 177,
            ItemsType.MaterialContainer => 177,
            ItemsType.Truck => 178,
            ItemsType.Van => 178,
            ItemsType.ContainerTruck => 177,
            ItemsType.ContainerTrailer => 177,
            ItemsType.FirstAidKit => 178,
            ItemsType.SurgicalKit => 178,
            ItemsType.MedicalRobot => 177,
            ItemsType.MedicalContainer => 177,
            ItemsType.ScaleMeter => 180,
            ItemsType.SearchProbe => 180,
            ItemsType.DroneWithGeologicalCamera => 180,
            ItemsType.InspectionContainer => 180,
            ItemsType.ReinforcedDrill => 180,
            ItemsType.SingleArmIndustrialRobot => 180,
            ItemsType.MultiArmIndustrialRobot => 180,
            ItemsType.HumanoidRobot => 180,
            _ => throw new Exception("Out of range"),
        };

        private static ICollection<ItemBonusDto> GenerateItemBonus(ItemsType type)
        {
            return type switch
            {
                ItemsType.AirHammer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 1 } },
                ItemsType.GravityHammer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 2 } },
                ItemsType.SonicHammer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 3 } },
                ItemsType.Excavator => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 4 } },
                ItemsType.Chainsaw => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 1 } },
                ItemsType.Gravitysaw => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 2 } },
                ItemsType.LaserCutter => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 3 } },
                ItemsType.Harvester => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 4 } },
                ItemsType.HuntingRifle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 1 } },
                ItemsType.RapidFireRifle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 2 } },
                ItemsType.SniperRifle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 3 } },
                ItemsType.OffRoadVehicleWithMachineGun => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 4 } },
                ItemsType.SeedlingContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 1 } },
                ItemsType.SeedlingKit => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 2 } },
                ItemsType.HydroponicVessel => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 3 } },
                ItemsType.HydroponicContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 4 } },
                ItemsType.AutomaticCarbine => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 1 } },
                ItemsType.LargeCaliberRifle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 2 } },
                ItemsType.LaserRifle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 3 } },
                ItemsType.ArmoredCarWithMachineGun => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 4 } },
                ItemsType.Binoculars => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Discovery, Bonus = 1 } },
                ItemsType.TacticalScope => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Discovery, Bonus = 2 } },
                ItemsType.NightVision => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Discovery, Bonus = 3 } },
                ItemsType.Motorcycle => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Discovery, Bonus = 4 } },
                ItemsType.MultiTool => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Repair, Bonus = 1 } },
                ItemsType.ToolBox => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Repair, Bonus = 2 } },
                ItemsType.Drill => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Repair, Bonus = 3 } },
                ItemsType.ContainerWithWorkshop => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Repair, Bonus = 4 } },
                ItemsType.HammerAndScrewdriver => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Construction, Bonus = 1 } },
                ItemsType.ElectronicMeter => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Construction, Bonus = 2 } },
                ItemsType.MultiFunctionRobot => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Construction, Bonus = 3 } },
                ItemsType.CraneOnTheCar => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Construction, Bonus = 4 } },
                ItemsType.HandProbe => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 1 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 1 } },
                ItemsType.LaserProbe => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 2 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 2 } },
                ItemsType.InspectionCrate => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 3 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 3 } },
                ItemsType.TestContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Meat, Bonus = 4 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Cereal, Bonus = 4 } },
                ItemsType.SetOfMeasuringCup => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 1 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 1 } },
                ItemsType.Distiller => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 2 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 2 } },
                ItemsType.DiagnosticStation => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 3 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 3 } },
                ItemsType.MaterialContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 4 }, new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Mining, Bonus = 4 } },
                ItemsType.Truck => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 1 } },
                ItemsType.Van => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 2 } },
                ItemsType.ContainerTruck => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 3 } },
                ItemsType.ContainerTrailer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 4 } },
                ItemsType.FirstAidKit => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Healing, Bonus = 1 } },
                ItemsType.SurgicalKit => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Healing, Bonus = 2 } },
                ItemsType.MedicalRobot => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Healing, Bonus = 3 } },
                ItemsType.MedicalContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Healing, Bonus = 4 } },
                ItemsType.ScaleMeter => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Detection, Bonus = 1 } },
                ItemsType.SearchProbe => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Detection, Bonus = 2 } },
                ItemsType.DroneWithGeologicalCamera => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Detection, Bonus = 3 } },
                ItemsType.InspectionContainer => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.Detection, Bonus = 4 } },
                ItemsType.ReinforcedDrill => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MechProduction, Bonus = 1 } },
                ItemsType.SingleArmIndustrialRobot => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MechProduction, Bonus = 2 } },
                ItemsType.MultiArmIndustrialRobot => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MechProduction, Bonus = 3 } },
                ItemsType.HumanoidRobot => new List<ItemBonusDto>() { new ItemBonusDto { ItemBonusTypeId = ItemBonusesType.MechProduction, Bonus = 4 } },
                _ => new List<ItemBonusDto>() { },
            };
        }
    }
}
