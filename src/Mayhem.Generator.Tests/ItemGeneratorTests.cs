using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Generator.Tests.Base;
using Mayhem.Package.Bl.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Generator.Tests
{
    public class ItemGeneratorTests : UnitTestBase
    {
        private IItemGeneratorService itemGeneratorService;

        [OneTimeSetUp]
        public void SetUp()
        {
            itemGeneratorService = GetService<IItemGeneratorService>();
        }

        [Test]
        public void GenerateItems_DisplayThem_Test()
        {
            List<ItemDto> items = itemGeneratorService.GenerateItems().ToList();

            StringBuilder stringBuilder = new();

            for (int i = 0; i < items.Count; i++)
            {
                stringBuilder.AppendLine($"Item: {i + 1} (Name: {items[i].Name}, Type: {items[i].ItemTypeId}), Bonus: {items[i].ItemBonuses.FirstOrDefault()?.ItemBonusTypeId}:{items[i].ItemBonuses.FirstOrDefault()?.Bonus}");
            }

            stringBuilder.AppendLine($"-----------------------------------------------------------------------------------");
            foreach (ItemsType values in Enum.GetValues(typeof(ItemsType)).Cast<ItemsType>())
            {
                stringBuilder.AppendLine($"{values}: {items.Where(x => x.ItemTypeId == values).Count()}");

            }

            stringBuilder.AppendLine($"Bonuses: {items?.Sum(x => x.ItemBonuses.Count)}");

            Console.WriteLine(stringBuilder.ToString());
            Assert.True(true);
        }
    }
}
