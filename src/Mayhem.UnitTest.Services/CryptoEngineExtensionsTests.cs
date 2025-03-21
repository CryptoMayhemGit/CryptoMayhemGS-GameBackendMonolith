using FluentAssertions;
using Mayhem.Helper;
using NUnit.Framework;
using System;
using System.Security.Cryptography;

namespace Mayhem.UnitTest.Services
{
    public class CryptoEngineExtensionsTests
    {
        private const string CryptoEngineKey = "q_t<w*k2zA!%JUdmw@]!A4R`";
        private const string ExpectedString = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";

        [Test]
        public void EncryptAndDecrypt_WhenCaseHasGoodValues_ThenValidateTrue_Test()
        {
            string encryptedString = ExpectedString.Encrypt(CryptoEngineKey);
            string decryptedString = encryptedString.Decrypt(CryptoEngineKey);

            encryptedString.Should().NotBe(ExpectedString);
            decryptedString.Should().Be(ExpectedString);
        }

        [Test]
        public void Encrypt_WhenCaseHasIncorrectInputToEncrypt_ThenValidateTrue_Test()
        {
            const string IncorrectEngineKey = ",";

            Func<string> function = () => ExpectedString.Encrypt(IncorrectEngineKey);

            function.Should().Throw<ArgumentException>().WithMessage("Specified key is not a valid size for this algorithm.");
        }

        [Test]
        public void Encrypt_WhenCaseHasIncorrectInputToDecrypt_ThenValidateTrue_Test()
        {
            const string IncorrectEngineKey = ",hUtTq?hsu4B{6;3";

            string encryptedString = ExpectedString.Encrypt(CryptoEngineKey);

            Func<string> function = () => ExpectedString.Decrypt(IncorrectEngineKey);
            function.Should().Throw<FormatException>().WithMessage("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.");
        }

        [Test]
        public void TryDecrypt_WhenCaseHasIncorrectInputToDecrypt_ThenGetFalse_Test()
        {
            const string IncorrectEngineKey = ",hUtTq?hsu4B{6;3";

            bool result = ExpectedString.TryDecrypt(IncorrectEngineKey);

            result.Should().BeFalse();
        }

        [Test]
        public void TryDecrypt_WhenCaseHasCorrectInputToDecrypt_ThenGetTrue_Test()
        {
            string encryptedString = ExpectedString.Encrypt(CryptoEngineKey);

            bool result = encryptedString.TryDecrypt(CryptoEngineKey);

            result.Should().BeTrue();
        }

        [Test]
        public void Encrypt_WhenCaseHasIncorrectKey_ThenValidateTrue_Test()
        {
            const string IncorrectEngineKey = ",hUtTq?hsu4B{6;3";

            string encryptedString = ExpectedString.Encrypt(CryptoEngineKey);

            Func<string> function = () => encryptedString.Decrypt(IncorrectEngineKey);
            function.Should().Throw<CryptographicException>().WithMessage("Padding is invalid and cannot be removed.");
        }
    }
}
