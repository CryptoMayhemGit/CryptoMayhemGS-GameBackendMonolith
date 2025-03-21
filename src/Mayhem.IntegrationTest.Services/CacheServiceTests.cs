using FluentAssertions;
using Mayhem.Cache.Extensions;
using Mayhem.Cache.Interfaces;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Services
{
    public class CacheServiceTests
    {
        private ICacheService cacheService;

        [SetUp]
        public void Setup()
        {
            IServiceCollection serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IConfiguration>(x => new ConfigurationBuilder().Build());
            IMayhemConfigurationService mayhemConfiguration = serviceProvider.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            serviceProvider.AddCache(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.CacheConnectionString, mayhemConfiguration.MayhemConfiguration.CommonConfiguration.CacheName);
            cacheService = serviceProvider.BuildServiceProvider().GetService<ICacheService>();
        }

        [Test]
        public async Task SetString_WhenStringAdded_ThenGetIt_Test()
        {
            string key = Guid.NewGuid().ToString();
            string value = Guid.NewGuid().ToString();

            try
            {
                await cacheService.SetStringAsync(key, value);
                string valueFromCache = await cacheService.GetStringAsync(key);

                valueFromCache.Should().Be(value);
            }
            finally
            {
                await cacheService.RemoveAsync(key);
            }
        }

        [Test]
        public async Task SetObject_WhenObjectAdded_ThenGetIt_Test()
        {
            string key = Guid.NewGuid().ToString();
            CacheServiceDto value = new("name", 15);

            try
            {
                await cacheService.SetObjectAsync(key, value);
                CacheServiceDto valueFromCache = await cacheService.GetObjectAsync<CacheServiceDto>(key);

                valueFromCache.Name.Should().Be(value.Name);
                valueFromCache.Age.Should().Be(value.Age);
            }
            finally
            {
                await cacheService.RemoveAsync(key);
            }
        }

        [Test]
        public async Task SetAndRemoveObject_WhenObjectRemoved_ThenGetIt_Test()
        {
            string key = Guid.NewGuid().ToString();
            CacheServiceDto value = new("name", 15);

            await cacheService.SetObjectAsync(key, value);
            await cacheService.RemoveAsync(key);
            CacheServiceDto valueFromCache = await cacheService.GetObjectAsync<CacheServiceDto>(key);

            valueFromCache.Should().BeNull();
        }

        [Test]
        public async Task SetAndRemoveString_WhenStringRemoved_ThenGetIt_Test()
        {
            string key = Guid.NewGuid().ToString();
            string value = Guid.NewGuid().ToString();

            await cacheService.SetStringAsync(key, value);
            await cacheService.RemoveAsync(key);
            string valueFromCache = await cacheService.GetStringAsync(key);

            valueFromCache.Should().BeNull();
        }
    }

    public class CacheServiceDto
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public CacheServiceDto(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}