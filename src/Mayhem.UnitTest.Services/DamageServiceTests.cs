using AutoMapper;
using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.UnitTest.Services
{
    public class DamageServiceTests : UnitTestBase
    {
        private IDamageService damageService;
        private IMapper mapper;

        private List<AttributeDto> attributes;
        private List<AttributeDto> attributesBase;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            damageService = GetService<IDamageService>();
            mapper = GetService<IMapper>();
        }

        [SetUp]
        public void Setup()
        {
            ICollection<Dal.Tables.Attribute> basicAttributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Biologist);
            attributes = basicAttributes.Select(x => mapper.Map<AttributeDto>(x)).ToList();
            attributesBase = basicAttributes.Select(x => mapper.Map<AttributeDto>(x)).ToList();
        }

        [Test]
        public void SetAttributeFromHealthyToWounded_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Healthy;
            NpcHealthsState newHealthsState = NpcHealthsState.Wounded;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value / 2d, 4));
            }
        }

        [Test]
        public void SetAttributeFromHealthyToDying_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Healthy;
            NpcHealthsState newHealthsState = NpcHealthsState.Dying;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value / 10d, 4));
            }
        }

        [Test]
        public void SetAttributeFromHealthyToHealthy_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Healthy;
            NpcHealthsState newHealthsState = NpcHealthsState.Healthy;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value, 4));
            }
        }

        [Test]
        public void SetAttributeFromWoundedToHealthy_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Wounded;
            NpcHealthsState newHealthsState = NpcHealthsState.Healthy;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value * 2d, 4));
            }
        }

        [Test]
        public void SetAttributeFromWoundedToWounded_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Wounded;
            NpcHealthsState newHealthsState = NpcHealthsState.Wounded;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value, 4));
            }
        }

        [Test]
        public void SetAttributeFromWoundedToDying_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Wounded;
            NpcHealthsState newHealthsState = NpcHealthsState.Dying;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value / 5d, 4));
            }
        }

        [Test]
        public void SetAttributeFromDyingToDying_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Dying;
            NpcHealthsState newHealthsState = NpcHealthsState.Dying;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value, 4));
            }
        }

        [Test]
        public void SetAttributeFromDyingToWounded_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Dying;
            NpcHealthsState newHealthsState = NpcHealthsState.Wounded;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value * 5d, 4));
            }
        }

        [Test]
        public void SetAttributeFromDyingToHealthy_WhenAttributesSet_ThenReduceValues_Test()
        {
            NpcHealthsState currentHealthsState = NpcHealthsState.Dying;
            NpcHealthsState newHealthsState = NpcHealthsState.Healthy;

            damageService.SetAttributesBasedOnHealth(attributes, currentHealthsState, newHealthsState);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value * 10d, 4));
            }
        }

        [Test]
        public void SetAttributeFromManyTimes_WhenAttributesSet_ThenReduceValues_Test()
        {
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Healthy, NpcHealthsState.Wounded);
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Wounded, NpcHealthsState.Dying);
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Dying, NpcHealthsState.Dying);
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Dying, NpcHealthsState.Wounded);
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Wounded, NpcHealthsState.Dying);
            damageService.SetAttributesBasedOnHealth(attributes, NpcHealthsState.Dying, NpcHealthsState.Healthy);

            foreach (AttributeDto item in attributes.Where(x => damageService.AttributesToEdit.Contains(x.AttributeTypeId)))
            {
                item.Value.Should().Be(Math.Round(attributesBase.Single(x => x.AttributeTypeId == item.AttributeTypeId).Value, 4));
            }
        }
    }
}

