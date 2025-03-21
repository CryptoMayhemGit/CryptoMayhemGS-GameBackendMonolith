using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;
using System.Text;

namespace Mayhem.Dal.Dto.Classes.Generator
{
    public class Package
    {
        public Package()
        {
            MatchingItems = new List<ItemDto>();
            MismatchingItems = new List<ItemDto>();
            Npcs = new List<NpcDto>();
        }

        public List<ItemDto> MatchingItems { get; set; }
        public List<ItemDto> MismatchingItems { get; set; }
        public List<NpcDto> Npcs { get; set; }

        public string ToPackageString(int packageNumber)
        {
            StringBuilder sb = new();
            sb.Append($"Paczka {packageNumber}: ")
            .Append($"{HeroToPolishName(Npcs[0].NpcTypeId)}({(Npcs[0].IsAvatar ? "Avatar" : "Npc")})")
            .Append($", {HeroToPolishName(Npcs[1].NpcTypeId)}({(Npcs[1].IsAvatar ? "Avatar" : "Npc")})")
            .Append($", {HeroToPolishName(Npcs[2].NpcTypeId)}({(Npcs[2].IsAvatar ? "Avatar" : "Npc")})")
            .Append($", {HeroToPolishName(Npcs[3].NpcTypeId)}({(Npcs[3].IsAvatar ? "Avatar" : "Npc")})")
            .Append(", Pasujące:")
            .Append($" {ItemToPolishName(MatchingItems[0].ItemTypeId)}")
            .Append($", {ItemToPolishName(MatchingItems[1].ItemTypeId)}")
            .Append($", {ItemToPolishName(MatchingItems[2].ItemTypeId)}")
            .Append($", {ItemToPolishName(MatchingItems[3].ItemTypeId)}")
            .Append(", Pozostałe:")
            .Append($" {ItemToPolishName(MismatchingItems[0].ItemTypeId)}")
            .Append($", {ItemToPolishName(MismatchingItems[1].ItemTypeId)}")
            .Append($", {ItemToPolishName(MismatchingItems[2].ItemTypeId)}")
            .Append($", {ItemToPolishName(MismatchingItems[3].ItemTypeId)}");

            return sb.ToString();
        }

        private static string HeroToPolishName(NpcsType type) => type switch
        {
            NpcsType.Miner => "Górnik",
            NpcsType.Lumberjack => "Drwal",
            NpcsType.Hunter => "Łowca",
            NpcsType.Farmer => "Farmer",
            NpcsType.Soldier => "Żołnierz",
            NpcsType.Scout => "Zwiadowca",
            NpcsType.Mechanic => "Mechanik",
            NpcsType.Engineer => "Inżynier",
            NpcsType.Biologist => "Biolog",
            NpcsType.Chemist => "Chemik",
            NpcsType.Pilot => "Pilot",
            NpcsType.Doctor => "Doktor",
            NpcsType.Geologist => "Geolog",
            NpcsType.Fitter => "Monter",
            _ => "",
        };

        private static string ItemToPolishName(ItemsType type) => type switch
        {
            ItemsType.AirHammer => "młot pneumatyczny",
            ItemsType.GravityHammer => "młot grawitacyjny",
            ItemsType.SonicHammer => "młot soniczny",
            ItemsType.Excavator => "koparka",
            ItemsType.Chainsaw => "piła łańcuchowa",
            ItemsType.Gravitysaw => "piła grawitacyjna",
            ItemsType.LaserCutter => "przecinak laserowy",
            ItemsType.Harvester => "harwester",
            ItemsType.HuntingRifle => "strzelba myśliwska",
            ItemsType.RapidFireRifle => "sztucer szybkostrzelny",
            ItemsType.SniperRifle => "karabin snajperski",
            ItemsType.OffRoadVehicleWithMachineGun => "samochód terenowy z KM",
            ItemsType.SeedlingContainer => "pojemnik z sadzonką",
            ItemsType.SeedlingKit => "zestaw sadzonek",
            ItemsType.HydroponicVessel => "pojemnik hydroponiczny",
            ItemsType.HydroponicContainer => "kontener hydroponiczny",
            ItemsType.AutomaticCarbine => "karabinek automatyczny",
            ItemsType.LargeCaliberRifle => "karabin wielkokalibrowy",
            ItemsType.LaserRifle => "karabin laserowy",
            ItemsType.ArmoredCarWithMachineGun => "samochód opancerzony z KM",
            ItemsType.Binoculars => "lornetka",
            ItemsType.TacticalScope => "luneta taktyczna",
            ItemsType.NightVision => "noktowizor",
            ItemsType.Motorcycle => "motocykl",
            ItemsType.MultiTool => "multi-tool",
            ItemsType.ToolBox => "skrzynka z narzędziami",
            ItemsType.Drill => "wiertarko - wkrętarka",
            ItemsType.ContainerWithWorkshop => "kontener z warsztatem",
            ItemsType.HammerAndScrewdriver => "młotek i śrubokręt",
            ItemsType.ElectronicMeter => "miernik elektroniczny",
            ItemsType.MultiFunctionRobot => "robot wielofunkcyjny",
            ItemsType.CraneOnTheCar => "dźwig na samochodzie",
            ItemsType.HandProbe => "sonda ręczna",
            ItemsType.LaserProbe => "sonda laserowa",
            ItemsType.InspectionCrate => "skrzynia inspekcyjna",
            ItemsType.TestContainer => "kontener badawczy",
            ItemsType.SetOfMeasuringCup => "zestaw menzurek",
            ItemsType.Distiller => "destylator",
            ItemsType.DiagnosticStation => "stacja diagnostyczna",
            ItemsType.MaterialContainer => "kontener materiałowy",
            ItemsType.Truck => "samochód ciężarowy",
            ItemsType.Van => "amochód dostawczy",
            ItemsType.ContainerTruck => "samochód do transportu kontenerów",
            ItemsType.ContainerTrailer => "przyczepa do transportu kontenerów",
            ItemsType.FirstAidKit => "apteczka",
            ItemsType.SurgicalKit => "zestaw chirurgiczny",
            ItemsType.MedicalRobot => "robot medyczny",
            ItemsType.MedicalContainer => "kontener medyczny",
            ItemsType.ScaleMeter => "miernik ze skalą",
            ItemsType.SearchProbe => "sonda poszukiwawcza",
            ItemsType.DroneWithGeologicalCamera => "dron z kamerą geologiczną",
            ItemsType.InspectionContainer => "kontener inspekcyjny",
            ItemsType.ReinforcedDrill => "wzmocniona wiertarka",
            ItemsType.SingleArmIndustrialRobot => "robot przemysłowy jednoramienny",
            ItemsType.MultiArmIndustrialRobot => "robot przemysłowy wieloramienny",
            ItemsType.HumanoidRobot => "robot humanoidalny",
            _ => "",
        };
    }
}
