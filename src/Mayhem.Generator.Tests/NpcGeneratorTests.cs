using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Generator.Tests.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mayhem.Package.Bl.Interfaces;

namespace Mayhem.Generator.Tests
{
    public class NpcGeneratorTests : UnitTestBase
    {
        private INpcGeneratorService npcGeneratorService;

        [OneTimeSetUp]
        public void SetUp()
        {
            npcGeneratorService = GetService<INpcGeneratorService>();
        }

        [Test]
        public void GenerateNpcs_DisplayThem_Test()
        {
            List<NpcDto> npcs = npcGeneratorService.GeneratNpcs().ToList();

            StringBuilder stringBuilder = new();

            for (int i = 0; i < npcs.Count; i++)
            {
                stringBuilder.AppendLine($"Npc: {i + 1} (Name: {npcs[i].Name}, Type: {npcs[i].NpcTypeId}), Attributes: {npcs[i].Attributes.Count}");
            }

            stringBuilder.AppendLine($"-----------------------------------------------------------------------------------");
            foreach (NpcsType values in Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>())
            {
                stringBuilder.AppendLine($"Npc - {values}: {npcs.Where(x => x.NpcTypeId == values && !x.IsAvatar).Count()}");
            }
            foreach (NpcsType values in Enum.GetValues(typeof(NpcsType)).Cast<NpcsType>())
            {
                stringBuilder.AppendLine($"Avatar - {values}: {npcs.Where(x => x.NpcTypeId == values && x.IsAvatar).Count()}");
            }

            stringBuilder.AppendLine($"Attributes: {npcs.Sum(x => x.Attributes.Count)}");

            Console.WriteLine(stringBuilder.ToString());
            Assert.True(true);
        }
    }
}
