using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Generator.Tests.Base;
using Mayhem.Package.Bl.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Generator.Tests
{
    public class PackageGeneratorTests : UnitTestBase
    {
        private const int PackageAmount = 1252;
        private const int AvatarAmount = 224;
        private const int NpcAmount = 4784;
        private const int ItemAmount = 10016;

        private IPackageGeneratorService packageGeneratorService;
        private IPackageService packageService;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            packageGeneratorService = GetService<IPackageGeneratorService>();
            packageService = GetService<IPackageService>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public void GeneratePackages_WhenPackagesGenerated_ThenGetThem_Test()
        {
            IEnumerable<Dal.Dto.Classes.Generator.Package> packages = packageGeneratorService.GeneratePackages(PackageAmount);

            packages.Should().HaveCount(PackageAmount);
            foreach (Dal.Dto.Classes.Generator.Package package in packages)
            {
                package.Npcs.Should().HaveCount(4);
                package.MismatchingItems.Should().HaveCount(4);
                package.MatchingItems.Should().HaveCount(4);
                package.Npcs.Where(x => x.IsAvatar).Should().HaveCountLessOrEqualTo(1);
                package.Npcs.Where(x => x.IsAvatar).Should().HaveCountGreaterOrEqualTo(0);
                package.Npcs.Select(x => x.NpcTypeId).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList().Count.Should().Be(0);
            }

            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar)).ToList().Should().HaveCount(AvatarAmount);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar)).ToList().Should().HaveCount(NpcAmount);
            packages.SelectMany(x => x.MatchingItems.Union(x.MismatchingItems)).ToList().Should().HaveCount(ItemAmount);

            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.AirHammer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.AirHammer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.GravityHammer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.GravityHammer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SonicHammer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SonicHammer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Excavator).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Excavator))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Chainsaw).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Chainsaw))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Gravitysaw).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Gravitysaw))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserCutter).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserCutter))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Harvester).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Harvester))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HuntingRifle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HuntingRifle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.RapidFireRifle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.RapidFireRifle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SniperRifle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SniperRifle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.OffRoadVehicleWithMachineGun).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.OffRoadVehicleWithMachineGun))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SeedlingContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SeedlingContainer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SeedlingKit).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SeedlingKit))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HydroponicVessel).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HydroponicVessel))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HydroponicContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HydroponicContainer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.AutomaticCarbine).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.AutomaticCarbine))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.LargeCaliberRifle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.LargeCaliberRifle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserRifle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserRifle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ArmoredCarWithMachineGun).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ArmoredCarWithMachineGun))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Binoculars).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Binoculars))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.TacticalScope).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.TacticalScope))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.NightVision).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.NightVision))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Motorcycle).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Motorcycle))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiTool).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiTool))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ToolBox).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ToolBox))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Drill).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Drill))).ToList().Should().HaveCount(176);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerWithWorkshop).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerWithWorkshop))).ToList().Should().HaveCount(176);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HammerAndScrewdriver).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HammerAndScrewdriver))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ElectronicMeter).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ElectronicMeter))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiFunctionRobot).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiFunctionRobot))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.CraneOnTheCar).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.CraneOnTheCar))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HandProbe).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HandProbe))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserProbe).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.LaserProbe))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.InspectionCrate).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.InspectionCrate))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.TestContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.TestContainer))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SetOfMeasuringCup).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SetOfMeasuringCup))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Distiller).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Distiller))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.DiagnosticStation).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.DiagnosticStation))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MaterialContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MaterialContainer))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Truck).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Truck))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.Van).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.Van))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerTruck).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerTruck))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerTrailer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ContainerTrailer))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.FirstAidKit).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.FirstAidKit))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SurgicalKit).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SurgicalKit))).ToList().Should().HaveCount(178);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MedicalRobot).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MedicalRobot))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MedicalContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MedicalContainer))).ToList().Should().HaveCount(177);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ScaleMeter).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ScaleMeter))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SearchProbe).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SearchProbe))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.DroneWithGeologicalCamera).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.DroneWithGeologicalCamera))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.InspectionContainer).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.InspectionContainer))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.ReinforcedDrill).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.ReinforcedDrill))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.SingleArmIndustrialRobot).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.SingleArmIndustrialRobot))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiArmIndustrialRobot).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.MultiArmIndustrialRobot))).ToList().Should().HaveCount(180);
            packages.SelectMany(x => x.MatchingItems.Where(x => x.ItemTypeId == ItemsType.HumanoidRobot).Union(x.MismatchingItems.Where(x => x.ItemTypeId == ItemsType.HumanoidRobot))).ToList().Should().HaveCount(180);

            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Miner)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Lumberjack)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Hunter)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Farmer)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Soldier)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Scout)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Mechanic)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Engineer)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Biologist)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Chemist)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Pilot)).ToList().Should().HaveCount(4 * 57 + 2 * 56);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Doctor)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Geologist)).ToList().Should().HaveCount(6 * 57);
            packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar && x.NpcTypeId == NpcsType.Fitter)).ToList().Should().HaveCount(4 * 57 + 2 * 56);

            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Miner)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Lumberjack)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Hunter)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Farmer)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Soldier)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Scout)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Mechanic)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Engineer)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Biologist)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Chemist)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Pilot)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Doctor)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Geologist)).ToList().Should().HaveCount(16);
            packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar && x.NpcTypeId == NpcsType.Fitter)).ToList().Should().HaveCount(16);

            Console.WriteLine("Success");
        }

        [Test]
        public void GeneratePackages_DisplayThem_Test()
        {
            List<Dal.Dto.Classes.Generator.Package> packages = packageGeneratorService.GeneratePackages(PackageAmount).ToList();

            StringBuilder stringBuilder = new();

            for (int i = 0; i < packages.Count; i++)
            {
                stringBuilder.AppendLine($"Package id: {i + 1}, Npcs: {string.Join("-", packages[i].Npcs.Select(x => $"{x.NpcTypeId}({(x.IsAvatar ? "Avatar" : "Npc")})"))}, matched items: {string.Join("-", packages[i].MatchingItems.Select(x => x.ItemTypeId))}, mismatched items: {string.Join("-", packages[i].MismatchingItems.Select(x => x.ItemTypeId))}");
            }

            stringBuilder.AppendLine($"-----------------------------------------------------------------------------------");

            Console.WriteLine(stringBuilder.ToString());
            Assert.True(true);
        }

        [Test]
        public async Task GeneratePackagesAndUpdateNftIds_WhenPackagesGenerated_ThenGetIt_Test()
        {
            StringBuilder sb1 = new();

            List<string> tempList = new();

            List<Dal.Dto.Classes.Generator.Package> packages = packageService.GenerateAndValidate(PackageAmount).ToList();
            bool result = await packageService.InsertNftsAsync(packages);

            List<Dal.Tables.Nfts.Item> items = await mayhemDataContext.Items.ToListAsync();
            List<Dal.Tables.Nfts.Npc> npcs = await mayhemDataContext.Npcs.ToListAsync();

            for (int i = 0, j = 0; j < packages.Count; i += 4, j++)
            {
                sb1.Append(npcs[i].NpcTypeId)
                  .Append(npcs[i].IsAvatar)
                  .Append(npcs[i + 1].NpcTypeId)
                  .Append(npcs[i + 1].IsAvatar)
                  .Append(npcs[i + 2].NpcTypeId)
                  .Append(npcs[i + 2].IsAvatar)
                  .Append(npcs[i + 3].NpcTypeId)
                  .Append(npcs[i + 3].IsAvatar);

                tempList.Add(sb1.ToString());
                sb1.Clear();
            }

            int counter = 0;
            for (int i = 0, j = 0; j < packages.Count; i += 8, j++)
            {
                sb1.Append(items[i].ItemTypeId)
                  .Append(items[i + 1].ItemTypeId)
                  .Append(items[i + 2].ItemTypeId)
                  .Append(items[i + 3].ItemTypeId)
                  .Append(items[i + 4].ItemTypeId)
                  .Append(items[i + 5].ItemTypeId)
                  .Append(items[i + 6].ItemTypeId)
                  .Append(items[i + 7].ItemTypeId);

                tempList[counter] += sb1.ToString();
                counter++;
                sb1.Clear();
            }

            for (int i = 0; i < packages.Count; i++)
            {
                sb1.Append(packages[i].Npcs[0].NpcTypeId)
                  .Append(packages[i].Npcs[0].IsAvatar)
                  .Append(packages[i].Npcs[1].NpcTypeId)
                  .Append(packages[i].Npcs[1].IsAvatar)
                  .Append(packages[i].Npcs[2].NpcTypeId)
                  .Append(packages[i].Npcs[2].IsAvatar)
                  .Append(packages[i].Npcs[3].NpcTypeId)
                  .Append(packages[i].Npcs[3].IsAvatar)
                  .Append(packages[i].MatchingItems[0].ItemTypeId)
                  .Append(packages[i].MatchingItems[1].ItemTypeId)
                  .Append(packages[i].MatchingItems[2].ItemTypeId)
                  .Append(packages[i].MatchingItems[3].ItemTypeId)
                  .Append(packages[i].MismatchingItems[0].ItemTypeId)
                  .Append(packages[i].MismatchingItems[1].ItemTypeId)
                  .Append(packages[i].MismatchingItems[2].ItemTypeId)
                  .Append(packages[i].MismatchingItems[3].ItemTypeId);

                string serializedPackage = tempList[i];
                string dbPackage = sb1.ToString();

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine(serializedPackage);
                Console.WriteLine(dbPackage);
                Console.WriteLine("---------------------------------------------------------");

                serializedPackage.Should().Be(dbPackage);

                sb1.Clear();
            }

            result.Should().BeTrue();
        }
    }
}
