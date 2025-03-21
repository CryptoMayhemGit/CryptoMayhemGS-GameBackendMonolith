using AutoMapper;
using Mayhem.ApplicationSetup;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Mayhem.UnitTes.Mapper
{
    public class MapperDtosTests
    {
        private IMapper mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddAutoMapperConfiguration();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            mapper = serviceProvider.GetService<IMapper>();
        }

        [Test]
        public void AutoMapperConfigurationsTest()
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}