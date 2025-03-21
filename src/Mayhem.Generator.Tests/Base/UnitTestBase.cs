using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Mock;
using Mayhem.Dal.Repositories;
using Mayhem.Generator.ApplicationSetup;
using Mayhem.Package.Bl.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Mayhem.Generator.Tests.Base
{
    public class UnitTestBase
    {
        private IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IMayhemDataContext>(x => new MayhemDataContextMock(Guid.NewGuid().ToString()));
            services.AddScoped(x => new Mock<ILogger<ItemGeneratorService>>().Object);
            services.AddScoped(x => new Mock<ILogger<NpcGeneratorService>>().Object);
            services.AddScoped(x => new Mock<ILogger<PackageGeneratorService>>().Object);
            services.AddScoped(x => new Mock<ILogger<PackageRepository>>().Object);
            services.AddScoped<IConfiguration>(x => new ConfigurationBuilder().Build());
            services.AddServicesForUnitTests();

            serviceProvider = services.BuildServiceProvider();
        }

        protected T GetService<T>()
            where T : class
        {
            return serviceProvider.GetService<T>();
        }
    }
}
