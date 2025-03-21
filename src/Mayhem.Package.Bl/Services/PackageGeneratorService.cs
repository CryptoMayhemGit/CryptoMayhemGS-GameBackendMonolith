using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Package.Bl.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Package.Bl.Services
{
    public class PackageGeneratorService : IPackageGeneratorService
    {
        private const int ItemAmount = 10016;
        private const int AvatarAmount = 224;
        private const int NpcAmount = 4784;

        private readonly ILogger<PackageGeneratorService> logger;

        private readonly IItemGeneratorService itemGeneratorService;
        private readonly INpcGeneratorService npcGeneratorService;

        public PackageGeneratorService(
            ILogger<PackageGeneratorService> logger,
            IItemGeneratorService itemGeneratorService,
            INpcGeneratorService npcGeneratorService)
        {
            this.logger = logger;
            this.itemGeneratorService = itemGeneratorService;
            this.npcGeneratorService = npcGeneratorService;
        }

        public IEnumerable<Dal.Dto.Classes.Generator.Package> GeneratePackages(int packageAmount)
        {
            List<ItemDto> itemsDb = itemGeneratorService.GenerateItems().ToList();
            List<NpcDto> npcDb = npcGeneratorService.GeneratNpcs().ToList();

            if (itemsDb.Count != ItemAmount ||
                npcDb.Where(x => x.IsAvatar).Count() != AvatarAmount ||
                npcDb.Where(x => !x.IsAvatar).Count() != NpcAmount)
            {
                throw new Exception("Wrong item/npc amount");
            }

            List<ItemDto> minerItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.AirHammer || x.ItemTypeId == ItemsType.GravityHammer || x.ItemTypeId == ItemsType.SonicHammer || x.ItemTypeId == ItemsType.Excavator).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> lumberjackItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.Chainsaw || x.ItemTypeId == ItemsType.Gravitysaw || x.ItemTypeId == ItemsType.LaserCutter || x.ItemTypeId == ItemsType.Harvester).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> hunterItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.HuntingRifle || x.ItemTypeId == ItemsType.RapidFireRifle || x.ItemTypeId == ItemsType.SniperRifle || x.ItemTypeId == ItemsType.OffRoadVehicleWithMachineGun).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> farmerItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.SeedlingContainer || x.ItemTypeId == ItemsType.SeedlingKit || x.ItemTypeId == ItemsType.HydroponicVessel || x.ItemTypeId == ItemsType.HydroponicContainer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> scoutItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.AutomaticCarbine || x.ItemTypeId == ItemsType.LargeCaliberRifle || x.ItemTypeId == ItemsType.LaserRifle || x.ItemTypeId == ItemsType.ArmoredCarWithMachineGun).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> soldierItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.Binoculars || x.ItemTypeId == ItemsType.TacticalScope || x.ItemTypeId == ItemsType.NightVision || x.ItemTypeId == ItemsType.Motorcycle).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> mechanicItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.MultiTool || x.ItemTypeId == ItemsType.ToolBox || x.ItemTypeId == ItemsType.Drill || x.ItemTypeId == ItemsType.ContainerWithWorkshop).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> engineerItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.HammerAndScrewdriver || x.ItemTypeId == ItemsType.ElectronicMeter || x.ItemTypeId == ItemsType.MultiFunctionRobot || x.ItemTypeId == ItemsType.CraneOnTheCar).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> biologistItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.HandProbe || x.ItemTypeId == ItemsType.LaserProbe || x.ItemTypeId == ItemsType.InspectionCrate || x.ItemTypeId == ItemsType.TestContainer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> chemistItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.SetOfMeasuringCup || x.ItemTypeId == ItemsType.Distiller || x.ItemTypeId == ItemsType.DiagnosticStation || x.ItemTypeId == ItemsType.MaterialContainer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> pilotItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.Truck || x.ItemTypeId == ItemsType.Van || x.ItemTypeId == ItemsType.ContainerTruck || x.ItemTypeId == ItemsType.ContainerTrailer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> doctorItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.FirstAidKit || x.ItemTypeId == ItemsType.SurgicalKit || x.ItemTypeId == ItemsType.MedicalRobot || x.ItemTypeId == ItemsType.MedicalContainer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> geologistItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.ScaleMeter || x.ItemTypeId == ItemsType.SearchProbe || x.ItemTypeId == ItemsType.DroneWithGeologicalCamera || x.ItemTypeId == ItemsType.InspectionContainer).OrderBy(x => Guid.NewGuid()).ToList();
            List<ItemDto> fitterItems = itemsDb.Where(x => x.ItemTypeId == ItemsType.ReinforcedDrill || x.ItemTypeId == ItemsType.SingleArmIndustrialRobot || x.ItemTypeId == ItemsType.MultiArmIndustrialRobot || x.ItemTypeId == ItemsType.HumanoidRobot).OrderBy(x => Guid.NewGuid()).ToList();

            List<NpcDto> avatarMiners = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Miner).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarLumberjacks = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Lumberjack).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarHunters = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Hunter).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarFarmers = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Farmer).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarSoldiers = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Soldier).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarScouts = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Scout).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarMechanics = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Mechanic).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarEngineers = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Engineer).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarBiologists = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Biologist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarChemists = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Chemist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarPilots = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Pilot).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarDoctors = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Doctor).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarGeologists = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Geologist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> avatarFitters = npcDb.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Fitter).OrderBy(x => Guid.NewGuid()).ToList();

            List<NpcDto> npcMiners = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Miner).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcLumberjacks = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Lumberjack).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcHunters = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Hunter).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcFarmers = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Farmer).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcSoldiers = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Soldier).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcScouts = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Scout).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcMechanics = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Mechanic).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcEngineers = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Engineer).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcBiologists = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Biologist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcChemists = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Chemist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcPilots = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Pilot).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcDoctors = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Doctor).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcGeologists = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Geologist).OrderBy(x => Guid.NewGuid()).ToList();
            List<NpcDto> npcFitters = npcDb.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Fitter).OrderBy(x => Guid.NewGuid()).ToList();

            Random random = new();
            List<Dal.Dto.Classes.Generator.Package> packages = new();

            List<int> packagesWithAvatars = GeneratePackagesWithAvatars(packageAmount);

            List<NpcsType> avatarsTypes = Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>().ToList();

            for (int packageIterator = 0; packageIterator < packageAmount; packageIterator++)
            {
                Dal.Dto.Classes.Generator.Package package = new();
                List<NpcsType> addedTypes = new();

                if (packagesWithAvatars.Contains(packageIterator))
                {
                    while (package.Npcs.Count != 1)
                    {
                        NpcsType pointer;
                        if (avatarsTypes.Count == 1)
                        {
                            pointer = avatarsTypes.First();
                        }
                        else
                        {
                            pointer = avatarsTypes[random.Next(0, avatarsTypes.Count)];
                        }

                        switch (pointer)
                        {
                            case NpcsType.Miner:
                                NpcDto avatarMiner = avatarMiners.FirstOrDefault();
                                if (avatarMiner == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Miner);
                                    continue;
                                }
                                package.Npcs.Add(avatarMiner);
                                avatarMiners.Remove(avatarMiner);
                                addedTypes.Add(NpcsType.Miner);
                                break;
                            case NpcsType.Lumberjack:
                                NpcDto avatarLumberjack = avatarLumberjacks.FirstOrDefault();
                                if (avatarLumberjack == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Lumberjack);
                                    continue;
                                }
                                package.Npcs.Add(avatarLumberjack);
                                avatarLumberjacks.Remove(avatarLumberjack);
                                addedTypes.Add(NpcsType.Lumberjack);
                                break;
                            case NpcsType.Hunter:
                                NpcDto avatarHunter = avatarHunters.FirstOrDefault();
                                if (avatarHunter == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Hunter);
                                    continue;
                                }
                                package.Npcs.Add(avatarHunter);
                                avatarHunters.Remove(avatarHunter);
                                addedTypes.Add(NpcsType.Hunter);
                                break;
                            case NpcsType.Farmer:
                                NpcDto avatarFarmer = avatarFarmers.FirstOrDefault();
                                if (avatarFarmer == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Farmer);
                                    continue;
                                }
                                package.Npcs.Add(avatarFarmer);
                                avatarFarmers.Remove(avatarFarmer);
                                addedTypes.Add(NpcsType.Farmer);
                                break;
                            case NpcsType.Soldier:
                                NpcDto avatarSoldier = avatarSoldiers.FirstOrDefault();
                                if (avatarSoldier == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Soldier);
                                    continue;
                                }
                                package.Npcs.Add(avatarSoldier);
                                avatarSoldiers.Remove(avatarSoldier);
                                addedTypes.Add(NpcsType.Soldier);
                                break;
                            case NpcsType.Scout:
                                NpcDto avatarScout = avatarScouts.FirstOrDefault();
                                if (avatarScout == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Scout);
                                    continue;
                                }
                                package.Npcs.Add(avatarScout);
                                avatarScouts.Remove(avatarScout);
                                addedTypes.Add(NpcsType.Scout);
                                break;
                            case NpcsType.Mechanic:
                                NpcDto avatarMechanic = avatarMechanics.FirstOrDefault();
                                if (avatarMechanic == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Mechanic);
                                    continue;
                                }
                                package.Npcs.Add(avatarMechanic);
                                avatarMechanics.Remove(avatarMechanic);
                                addedTypes.Add(NpcsType.Mechanic);
                                break;
                            case NpcsType.Engineer:
                                NpcDto avatarEngineer = avatarEngineers.FirstOrDefault();
                                if (avatarEngineer == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Engineer);
                                    continue;
                                }
                                package.Npcs.Add(avatarEngineer);
                                avatarEngineers.Remove(avatarEngineer);
                                addedTypes.Add(NpcsType.Engineer);
                                break;
                            case NpcsType.Biologist:
                                NpcDto avatarBiologist = avatarBiologists.FirstOrDefault();
                                if (avatarBiologist == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Biologist);
                                    continue;
                                }
                                package.Npcs.Add(avatarBiologist);
                                avatarBiologists.Remove(avatarBiologist);
                                addedTypes.Add(NpcsType.Biologist);
                                break;
                            case NpcsType.Chemist:
                                NpcDto avatarChemist = avatarChemists.FirstOrDefault();
                                if (avatarChemist == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Chemist);
                                    continue;
                                }
                                package.Npcs.Add(avatarChemist);
                                avatarChemists.Remove(avatarChemist);
                                addedTypes.Add(NpcsType.Chemist);
                                break;
                            case NpcsType.Pilot:
                                NpcDto avatarPilot = avatarPilots.FirstOrDefault();
                                if (avatarPilot == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Pilot);
                                    continue;
                                }
                                package.Npcs.Add(avatarPilot);
                                avatarPilots.Remove(avatarPilot);
                                addedTypes.Add(NpcsType.Pilot);
                                break;
                            case NpcsType.Doctor:
                                NpcDto avatarDoctor = avatarDoctors.FirstOrDefault();
                                if (avatarDoctor == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Doctor);
                                    continue;
                                }
                                package.Npcs.Add(avatarDoctor);
                                avatarDoctors.Remove(avatarDoctor);
                                addedTypes.Add(NpcsType.Doctor);
                                break;
                            case NpcsType.Geologist:
                                NpcDto avatarGeologist = avatarGeologists.FirstOrDefault();
                                if (avatarGeologist == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Geologist);
                                    continue;
                                }
                                package.Npcs.Add(avatarGeologist);
                                avatarGeologists.Remove(avatarGeologist);
                                addedTypes.Add(NpcsType.Geologist);
                                break;
                            case NpcsType.Fitter:
                                NpcDto avatarFitter = avatarFitters.FirstOrDefault();
                                if (avatarFitter == null)
                                {
                                    avatarsTypes.Remove(NpcsType.Fitter);
                                    continue;
                                }
                                package.Npcs.Add(avatarFitter);
                                avatarFitters.Remove(avatarFitter);
                                addedTypes.Add(NpcsType.Fitter);
                                break;
                            default:
                                break;
                        }
                    }
                }

                while (package.Npcs.Count != 4)
                {
                    NpcsType pointer = GetNextNpcType(addedTypes,
                    npcMiners, npcLumberjacks, npcHunters, npcFarmers, npcScouts,
                    npcSoldiers, npcMechanics, npcEngineers, npcBiologists,
                    npcChemists, npcPilots, npcDoctors, npcGeologists, npcFitters);


                    switch (pointer)
                    {
                        case NpcsType.Miner:
                            NpcDto npcMiner = npcMiners.FirstOrDefault();
                            package.Npcs.Add(npcMiner);
                            npcMiners.Remove(npcMiner);
                            addedTypes.Add(NpcsType.Miner);
                            break;
                        case NpcsType.Lumberjack:
                            NpcDto npcLumberjack = npcLumberjacks.FirstOrDefault();
                            package.Npcs.Add(npcLumberjack);
                            npcLumberjacks.Remove(npcLumberjack);
                            addedTypes.Add(NpcsType.Lumberjack);
                            break;
                        case NpcsType.Hunter:
                            NpcDto npcHunter = npcHunters.FirstOrDefault();
                            package.Npcs.Add(npcHunter);
                            npcHunters.Remove(npcHunter);
                            addedTypes.Add(NpcsType.Hunter);
                            break;
                        case NpcsType.Farmer:
                            NpcDto npcFarmer = npcFarmers.FirstOrDefault();
                            package.Npcs.Add(npcFarmer);
                            npcFarmers.Remove(npcFarmer);
                            addedTypes.Add(NpcsType.Farmer);
                            break;
                        case NpcsType.Soldier:
                            NpcDto npcSoldier = npcSoldiers.FirstOrDefault();
                            package.Npcs.Add(npcSoldier);
                            npcSoldiers.Remove(npcSoldier);
                            addedTypes.Add(NpcsType.Soldier);
                            break;
                        case NpcsType.Scout:
                            NpcDto npcScout = npcScouts.FirstOrDefault();
                            package.Npcs.Add(npcScout);
                            npcScouts.Remove(npcScout);
                            addedTypes.Add(NpcsType.Scout);
                            break;
                        case NpcsType.Mechanic:
                            NpcDto npcMechanic = npcMechanics.FirstOrDefault();
                            package.Npcs.Add(npcMechanic);
                            npcMechanics.Remove(npcMechanic);
                            addedTypes.Add(NpcsType.Mechanic);
                            break;
                        case NpcsType.Engineer:
                            NpcDto npcEngineer = npcEngineers.FirstOrDefault();
                            package.Npcs.Add(npcEngineer);
                            npcEngineers.Remove(npcEngineer);
                            addedTypes.Add(NpcsType.Engineer);
                            break;
                        case NpcsType.Biologist:
                            NpcDto npcBiologist = npcBiologists.FirstOrDefault();
                            package.Npcs.Add(npcBiologist);
                            npcBiologists.Remove(npcBiologist);
                            addedTypes.Add(NpcsType.Biologist);
                            break;
                        case NpcsType.Chemist:
                            NpcDto npcChemist = npcChemists.FirstOrDefault();
                            package.Npcs.Add(npcChemist);
                            npcChemists.Remove(npcChemist);
                            addedTypes.Add(NpcsType.Chemist);
                            break;
                        case NpcsType.Pilot:
                            NpcDto npcPilot = npcPilots.FirstOrDefault();
                            package.Npcs.Add(npcPilot);
                            npcPilots.Remove(npcPilot);
                            addedTypes.Add(NpcsType.Pilot);
                            break;
                        case NpcsType.Doctor:
                            NpcDto npcDoctor = npcDoctors.FirstOrDefault();
                            package.Npcs.Add(npcDoctor);
                            npcDoctors.Remove(npcDoctor);
                            addedTypes.Add(NpcsType.Doctor);
                            break;
                        case NpcsType.Geologist:
                            NpcDto npcGeologist = npcGeologists.FirstOrDefault();
                            package.Npcs.Add(npcGeologist);
                            npcGeologists.Remove(npcGeologist);
                            addedTypes.Add(NpcsType.Geologist);
                            break;
                        case NpcsType.Fitter:
                            NpcDto npcFitter = npcFitters.FirstOrDefault();
                            package.Npcs.Add(npcFitter);
                            npcFitters.Remove(npcFitter);
                            addedTypes.Add(NpcsType.Fitter);
                            break;
                        default:
                            break;
                    }
                }

                // get 4 matched items
                foreach (NpcDto npc in package.Npcs)
                {
                    switch (npc.NpcTypeId)
                    {
                        case NpcsType.Miner:
                            ItemDto minerItem = minerItems.First();
                            package.MatchingItems.Add(minerItem);
                            minerItems.Remove(minerItem);
                            break;
                        case NpcsType.Lumberjack:
                            ItemDto lumberjackItem = lumberjackItems.First();
                            package.MatchingItems.Add(lumberjackItem);
                            lumberjackItems.Remove(lumberjackItem);
                            break;
                        case NpcsType.Hunter:
                            ItemDto hunterItem = hunterItems.First();
                            package.MatchingItems.Add(hunterItem);
                            hunterItems.Remove(hunterItem);
                            break;
                        case NpcsType.Farmer:
                            ItemDto farmerItem = farmerItems.First();
                            package.MatchingItems.Add(farmerItem);
                            farmerItems.Remove(farmerItem);
                            break;
                        case NpcsType.Soldier:
                            ItemDto soldierItem = soldierItems.First();
                            package.MatchingItems.Add(soldierItem);
                            soldierItems.Remove(soldierItem);
                            break;
                        case NpcsType.Scout:
                            ItemDto scoutItem = scoutItems.First();
                            package.MatchingItems.Add(scoutItem);
                            scoutItems.Remove(scoutItem);
                            break;
                        case NpcsType.Mechanic:
                            ItemDto mechanicItem = mechanicItems.First();
                            package.MatchingItems.Add(mechanicItem);
                            mechanicItems.Remove(mechanicItem);
                            break;
                        case NpcsType.Engineer:
                            ItemDto engineerItem = engineerItems.First();
                            package.MatchingItems.Add(engineerItem);
                            engineerItems.Remove(engineerItem);
                            break;
                        case NpcsType.Biologist:
                            ItemDto biologistItem = biologistItems.First();
                            package.MatchingItems.Add(biologistItem);
                            biologistItems.Remove(biologistItem);
                            break;
                        case NpcsType.Chemist:
                            ItemDto chemistItem = chemistItems.First();
                            package.MatchingItems.Add(chemistItem);
                            chemistItems.Remove(chemistItem);
                            break;
                        case NpcsType.Pilot:
                            ItemDto pilotItem = pilotItems.First();
                            package.MatchingItems.Add(pilotItem);
                            pilotItems.Remove(pilotItem);
                            break;
                        case NpcsType.Doctor:
                            ItemDto doctorItem = doctorItems.First();
                            package.MatchingItems.Add(doctorItem);
                            doctorItems.Remove(doctorItem);
                            break;
                        case NpcsType.Geologist:
                            ItemDto geologistItem = geologistItems.First();
                            package.MatchingItems.Add(geologistItem);
                            geologistItems.Remove(geologistItem);
                            break;
                        case NpcsType.Fitter:
                            ItemDto fitterItem = fitterItems.First();
                            package.MatchingItems.Add(fitterItem);
                            fitterItems.Remove(fitterItem);
                            break;
                        default:
                            break;
                    }
                }

                packages.Add(package);
            }

            List<NpcsType> mismatchedNpcsTypes = Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>().ToList();

            foreach (Dal.Dto.Classes.Generator.Package package in packages)
            {
                // get 4 mismatched items
                while (package.MismatchingItems.Count != 4)
                {
                    NpcsType pointer;
                    if (mismatchedNpcsTypes.Count == 1)
                    {
                        pointer = mismatchedNpcsTypes.First();
                    }
                    else
                    {
                        pointer = mismatchedNpcsTypes[random.Next(0, mismatchedNpcsTypes.Count)];
                    }

                    switch (pointer)
                    {
                        case NpcsType.Miner:
                            ItemDto minerItem = minerItems.FirstOrDefault();
                            if (minerItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Miner);
                                continue;
                            }
                            package.MismatchingItems.Add(minerItem);
                            minerItems.Remove(minerItem);
                            break;
                        case NpcsType.Lumberjack:
                            ItemDto lumberjackItem = lumberjackItems.FirstOrDefault();
                            if (lumberjackItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Lumberjack);
                                continue;
                            }
                            package.MismatchingItems.Add(lumberjackItem);
                            lumberjackItems.Remove(lumberjackItem);
                            break;
                        case NpcsType.Hunter:
                            ItemDto hunterItem = hunterItems.FirstOrDefault();
                            if (hunterItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Hunter);
                                continue;
                            }
                            package.MismatchingItems.Add(hunterItem);
                            hunterItems.Remove(hunterItem);
                            break;
                        case NpcsType.Farmer:
                            ItemDto farmerItem = farmerItems.FirstOrDefault();
                            if (farmerItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Farmer);
                                continue;
                            }
                            package.MismatchingItems.Add(farmerItem);
                            farmerItems.Remove(farmerItem);
                            break;
                        case NpcsType.Soldier:
                            ItemDto soldierItem = soldierItems.FirstOrDefault();
                            if (soldierItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Soldier);
                                continue;
                            }
                            package.MismatchingItems.Add(soldierItem);
                            soldierItems.Remove(soldierItem);
                            break;
                        case NpcsType.Scout:
                            ItemDto scoutItem = scoutItems.FirstOrDefault();
                            if (scoutItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Scout);
                                continue;
                            }
                            package.MismatchingItems.Add(scoutItem);
                            scoutItems.Remove(scoutItem);
                            break;
                        case NpcsType.Mechanic:
                            ItemDto mechanicItem = mechanicItems.FirstOrDefault();
                            if (mechanicItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Mechanic);
                                continue;
                            }
                            package.MismatchingItems.Add(mechanicItem);
                            mechanicItems.Remove(mechanicItem);
                            break;
                        case NpcsType.Engineer:
                            ItemDto engineerItem = engineerItems.FirstOrDefault();
                            if (engineerItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Engineer);
                                continue;
                            }
                            package.MismatchingItems.Add(engineerItem);
                            engineerItems.Remove(engineerItem);
                            break;
                        case NpcsType.Biologist:
                            ItemDto biologistItem = biologistItems.FirstOrDefault();
                            if (biologistItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Biologist);
                                continue;
                            }
                            package.MismatchingItems.Add(biologistItem);
                            biologistItems.Remove(biologistItem);
                            break;
                        case NpcsType.Chemist:
                            ItemDto chemistItem = chemistItems.FirstOrDefault();
                            if (chemistItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Chemist);
                                continue;
                            }
                            package.MismatchingItems.Add(chemistItem);
                            chemistItems.Remove(chemistItem);
                            break;
                        case NpcsType.Pilot:
                            ItemDto pilotItem = pilotItems.FirstOrDefault();
                            if (pilotItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Pilot);
                                continue;
                            }
                            package.MismatchingItems.Add(pilotItem);
                            pilotItems.Remove(pilotItem);
                            break;
                        case NpcsType.Doctor:
                            ItemDto doctorItem = doctorItems.FirstOrDefault();
                            if (doctorItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Doctor);
                                continue;
                            }
                            package.MismatchingItems.Add(doctorItem);
                            doctorItems.Remove(doctorItem);
                            break;
                        case NpcsType.Geologist:
                            ItemDto geologistItem = geologistItems.FirstOrDefault();
                            if (geologistItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Geologist);
                                continue;
                            }
                            package.MismatchingItems.Add(geologistItem);
                            geologistItems.Remove(geologistItem);
                            break;
                        case NpcsType.Fitter:
                            ItemDto fitterItem = fitterItems.FirstOrDefault();
                            if (fitterItem == null)
                            {
                                mismatchedNpcsTypes.Remove(NpcsType.Fitter);
                                continue;
                            }
                            package.MismatchingItems.Add(fitterItem);
                            fitterItems.Remove(fitterItem);
                            break;
                        default:
                            break;
                    }
                }
            }

            StringBuilder stringBuilder = new();
            for (int i = 0; i < packages.Count; i++)
            {
                stringBuilder.AppendLine($"Generated package {i + 1}")
                .AppendLine($"Npcs: {string.Join(", ", packages[i].Npcs.Select(x => x.NpcTypeId))}")
                .AppendLine($"Mismatching items: {string.Join(", ", packages[i].MismatchingItems.Select(x => x.ItemTypeId))}")
                .AppendLine($"Matching items: {string.Join(", ", packages[i].MatchingItems.Select(x => x.ItemTypeId))}")
                .AppendLine("---------------------------------------------------------------------");
            }

            logger.LogInformation(stringBuilder.ToString());

            return packages.OrderBy(x => Guid.NewGuid());
        }

        private static NpcsType GetNextNpcType(List<NpcsType> addedTypes, params List<NpcDto>[] npcList)
        {
            try
            {
                List<List<NpcDto>> listofLists = new(npcList);

                int max = listofLists.Where(x => !x.Select(z => z.NpcTypeId).Any(addedTypes.Contains)).Select(x => x.Count).Max();
                List<List<NpcDto>> maxs = listofLists.Where(x => x.Count == max && !x.Select(z => z.NpcTypeId).Any(addedTypes.Contains)).ToList();

                int i = new Random().Next(maxs.Count);

                NpcsType type = maxs[i].First().NpcTypeId;

                return type;
            }
            catch (Exception)
            {
                return NpcsType.Scout;
            }
        }

        private static List<int> GeneratePackagesWithAvatars(int packageAmount)
        {
            Random random = new();
            List<int> packagesWithAvatars = new();

            while (packagesWithAvatars.Count != AvatarAmount)
            {
                int position = random.Next(1, packageAmount);
                if (packagesWithAvatars.Contains(position))
                {
                    continue;
                }

                packagesWithAvatars.Add(position);
            }
            packagesWithAvatars.Sort();

            return packagesWithAvatars;
        }
    }
}
