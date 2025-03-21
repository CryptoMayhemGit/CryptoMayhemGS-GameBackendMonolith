using Mayhem.ApplicationSetup;
using Mayhem.Blockchain.Implementations.Services;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Mock;
using Mayhem.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Mayhem.UnitTest.Base
{
    public class UnitTestBase
    {
        private IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(x => new ConfigurationBuilder().Build());
            IMayhemConfigurationService mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            services.AddScoped<IMayhemDataContext>(x => new MayhemDataContextMock(Guid.NewGuid().ToString()));
            services.AddScoped(x => new Mock<ILogger<BlockchainService>>().Object);
            services.AddScoped(x => new Mock<ILogger<NotificationRepository>>().Object);
            services.AddScoped(x => new Mock<ILogger<PackageRepository>>().Object);
            services.AddServicesForUnitTests(mayhemConfiguration);

            serviceProvider = services.BuildServiceProvider();
        }

        protected T GetService<T>()
            where T : class
        {
            return serviceProvider.GetService<T>();
        }
    }
}
