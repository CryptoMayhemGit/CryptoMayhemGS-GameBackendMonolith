using FluentAssertions;
using Mayhem.BlobStorage.Extensions;
using Mayhem.BlobStorage.Interfaces;
using Mayhem.BlobStorage.Services;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Services
{
    public class StorageServiceTests
    {
        private const string ContainerName = "mayhem";
        private IStorageService storageService;

        [SetUp]
        public void Setup()
        {
            IServiceCollection serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IConfiguration>(x => new ConfigurationBuilder().Build());
            IMayhemConfigurationService mayhemConfiguration = serviceProvider.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            serviceProvider.AddBlobStorage(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureBlobStorageConnectionString);
            serviceProvider.AddScoped(x => new Mock<ILogger<StorageService>>().Object);
            storageService = serviceProvider.BuildServiceProvider().GetService<IStorageService>();
        }

        [Test]
        public async Task GetBlob_WhenBlobExist_ThenGetIt_Test()
        {
            string name = Guid.NewGuid().ToString();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testfile.txt");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "example text");
            }

            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    FormFile file = new(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    await storageService.UploadBlobAsync(name, file, ContainerName);
                    string value = storageService.GetBlob(name, ContainerName);
                    value.Should().Be($"https://kielsonstorage.blob.core.windows.net/mayhem/{name}");
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                await storageService.DeleteBlobAsync(name, ContainerName);
            }
        }

        [Test]
        public async Task DownloadBlob_WhenBlobExist_ThenGetIt_Test()
        {
            string name = Guid.NewGuid().ToString();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testfile.txt");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "example text");
            }

            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    FormFile file = new(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    await storageService.UploadBlobAsync(name, file, ContainerName);
                    string value = await storageService.DownloadBlobAsync(name, ContainerName);
                    value.Should().Be("example text");
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                await storageService.DeleteBlobAsync(name, ContainerName);
            }
        }

        [Test]
        public async Task GetBlobs_WhenBlobExist_ThenGetThem_Test()
        {
            string name1 = "a" + Guid.NewGuid().ToString();
            string name2 = "b" + Guid.NewGuid().ToString();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testfile.txt");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "example text");
            }

            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    FormFile file = new(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    await storageService.UploadBlobAsync(name1, file, ContainerName);
                    await storageService.UploadBlobAsync(name2, file, ContainerName);
                    List<string> values = (await storageService.GetBlobsNameAsync(ContainerName)).ToList();

                    values.Should().HaveCount(2);
                    values[0].Should().Be(name1);
                    values[1].Should().Be(name2);
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                await storageService.DeleteBlobAsync(name1, ContainerName);
                await storageService.DeleteBlobAsync(name2, ContainerName);
            }
        }
    }
}
