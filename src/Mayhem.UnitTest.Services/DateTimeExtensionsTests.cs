using FluentAssertions;
using Mayhem.Helper;
using NUnit.Framework;
using System;

namespace Mayhem.UnitTest.Services
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ConvertDateTimeToNonce_WhenConverter_ThenShouldBeSuccess_Test()
        {
            DateTime date = new(2022, 4, 10, 7, 23, 26, DateTimeKind.Utc);
            long nonce = date.ToUnixTime();

            nonce.Should().Be(1649575406);
        }

        [Test]
        public void ConvertNonceToDateTime_WhenConverter_ThenShouldBeSuccess_Test()
        {
            long nonce = 2642395820;
            DateTime date = nonce.FromUnixTime();

            date.Should().Be(new DateTime(2053, 09, 25, 6, 50, 20, DateTimeKind.Utc));
        }
    }
}
