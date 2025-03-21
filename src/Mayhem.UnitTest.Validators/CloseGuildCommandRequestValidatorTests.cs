using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.CloseGuild;
using Mayhen.Bl.Validators;
using NUnit.Framework;
using System.Linq;

namespace Mayhem.UnitTest.Validators
{
    public class CloseGuildCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public void CloseGuild_WhenOwnerNotExist_ThenThrowException_Test()
        {
            CloseGuildCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new CloseGuildCommandRequest(234234));

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("User isn't guild owner.");
            result.Errors.First().PropertyName.Should().Be("UserId");
        }
    }
}
