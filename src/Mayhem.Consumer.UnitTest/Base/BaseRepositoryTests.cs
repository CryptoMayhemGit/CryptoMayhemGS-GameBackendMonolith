using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Mayhem.Consumer.UnitTest.Base
{
    public class BaseRepositoryTests
    {
        protected IMayhemConfigurationService mayhemConfiguration;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(x => new ConfigurationBuilder().Build());
            mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
        }
    }
}