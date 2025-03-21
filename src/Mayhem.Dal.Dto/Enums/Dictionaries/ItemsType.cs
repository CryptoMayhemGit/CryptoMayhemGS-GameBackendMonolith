namespace Mayhem.Dal.Dto.Enums.Dictionaries
{
    public enum ItemsType
    {
        // Górnik - Kopalnia (Żelazo i Tytanium)
        AirHammer = 1, // młot pneumatyczny
        GravityHammer, // młot grawitacyjny
        SonicHammer, // młot soniczny
        Excavator, // koparka

        // Drwal - Tartak(Lekkie Drewno i Ciężkie Drewno)
        Chainsaw, // piła łańcuchowa
        Gravitysaw, // piła grawitacyjna
        LaserCutter, // przecinak laserowy
        Harvester, // harwester

        // Myśliwy - Mięso
        HuntingRifle, // strzelba myśliwska
        RapidFireRifle, // sztucer szybkostrzelny
        SniperRifle, // karabin snajperski
        OffRoadVehicleWithMachineGun, // samochód terenowy z KM (technical)

        // Farmer - Zboże
        SeedlingContainer, // pojemnik z sadzonką
        SeedlingKit, // zestaw sadzonek
        HydroponicVessel, // pojemnik hydroponiczny
        HydroponicContainer, // kontener hydroponiczny

        // Zwiadowca - Odkrywanie
        AutomaticCarbine, // karabinek automatyczny
        LargeCaliberRifle, // karabin wielkokalibrowy
        LaserRifle, // karabin laserowy
        ArmoredCarWithMachineGun, // samochód opancerzony z KM

        // Zołnierz - Atak
        Binoculars, // lornetka
        TacticalScope, // luneta taktyczna
        NightVision, // noktowizor
        Motorcycle, // motocykl

        // Mechanik - Naprawianie
        MultiTool, // multi-tool
        ToolBox, // skrzynka z narzędziami
        Drill, // wiertarko - wkrętarka
        ContainerWithWorkshop, // kontener z warsztatem

        // Inżynier - Budowanie
        HammerAndScrewdriver, // młotek i śrubokręt
        ElectronicMeter, // miernik elektroniczny
        MultiFunctionRobot, // robot wielofunkcyjny
        CraneOnTheCar, // dźwig na samochodzie

        // Biolog - Zboże i Mięso
        HandProbe, // sonda ręczna
        LaserProbe, // sonda laserowa
        InspectionCrate, // skrzynia inspekcyjna
        TestContainer, // kontener badawczy

        // Chemik - Lekkie Drewno, Twarde Drewno, Żelazo i Tytanium
        SetOfMeasuringCup, // zestaw menzurek
        Distiller, // destylator
        DiagnosticStation, // stacja diagnostyczna
        MaterialContainer, // kontener materiałowy

        // Pilot - Transport
        Truck, // samochód ciężarowy
        Van, // samochód dostawczy
        ContainerTruck, // samochód do transportu kontenerów
        ContainerTrailer, // przyczepa do transportu kontenerów

        // Lekarz - Gojenie Ran
        FirstAidKit, // apteczka
        SurgicalKit, // zestaw chirurgiczny
        MedicalRobot, // robot medyczny
        MedicalContainer, // kontener medyczny

        // Geolog - Wykrywanie
        ScaleMeter, // miernik ze skalą
        SearchProbe, // sonda poszukiwawcza
        DroneWithGeologicalCamera, // dron z kamerą geologiczną
        InspectionContainer, // kontener inspekcyjny

        // Monter - Produkcja Mechów
        ReinforcedDrill, // wzmocniona wiertarka
        SingleArmIndustrialRobot, // robot przemysłowy jednoramienny
        MultiArmIndustrialRobot, // robot przemysłowy wieloramienny
        HumanoidRobot, // robot humanoidalny
    }
}
