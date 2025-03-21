using FluentAssertions;
using Mayhem.Consumer.UnitTest.Base;
using NUnit.Framework;

namespace Mayhem.Consumer.UnitTest.Tests.Services
{
    public class ApplicationConfigurationExtensionsTests : BaseRepositoryTests
    {
        [Test]
        public void AddMayhemConfigurationService_WhenDependencyInjectionWorks_ThenGetMayhemConfiguration_Test()
        {
            mayhemConfiguration.MayhemConfiguration.Should().NotBeNull();

            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.GetLogsTopic.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.HttpClientServicePostTimeout.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.MaxBlocksToProcessed.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftItemDescription.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftItemSmartContractAddress.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftLandDescription.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftLandSmartContractAddress.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftNpcDescription.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NftNpcSmartContractAddress.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.NonceLifetimeInMinutes.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.TokenLifetimeInMinutes.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.CommonConfiguration.TransferIntervalInSeconds.Should().BeGreaterThan(0);

            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AppInsightInstrumentationKeyAPP.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueItemConnectionString.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueLandConnectionString.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueNotificationConnectionString.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueNpcConnectionString.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString.Should().NotBeNull();

            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.PackageAmount.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.LandNftAmount.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.LandsPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandForrestAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandForrestAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandMountainsAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandMountainsAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandFieldAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandFieldAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandFarmAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandFarmAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandUrbanAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandUrbanAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandRuinsAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandRuinsAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MinLandWaterAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.MaxLandWaterAmountPerInstance.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.NftItemCreations.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.NftNpcCreations.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.NftAvatarCreations.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.GeneratorConfiguration.NftAvatarAmount.Should().BeGreaterThan(0);

            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpSenderName.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpSenderAddress.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpHostAddress.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpHostPort.Should().BeGreaterThan(0);
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpHostUser.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.SmtpHostPass.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.NotificationConfigruation.ContactEmail.Should().NotBeNull();

            mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.BscscanApiEndpoint.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.MayhemApiEndpoint.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.Web3ProviderEndpoint.Should().NotBeNull();

            mayhemConfiguration.MayhemConfiguration.ServiceSecretsConfigruation.Should().NotBeNull();

            mayhemConfiguration.MayhemConfiguration.ServiceSecretsConfigruation.JwtIssuer.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceSecretsConfigruation.JwtAudience.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceSecretsConfigruation.JwtKey.Should().NotBeNull();
            mayhemConfiguration.MayhemConfiguration.ServiceSecretsConfigruation.ActivationTokenSecretKey.Should().NotBeNull();
        }
    }
}