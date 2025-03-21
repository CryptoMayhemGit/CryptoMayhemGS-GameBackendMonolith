using Mayhem.Blockchain.Implementations.Services;
using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Logger;
using Mayhem.Messages;
using Mayhem.Queue.Publisher.Extensions;
using Mayhem.Worker.Base.Interfaces;
using Mayhem.Worker.Base.Services;
using Mayhem.Worker.TransferListener;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Workers.Dal.Repositories.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nethereum.Web3;
using Serilog;
using System;

namespace Mayhem.Avatar.Worker.App
{
    internal class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace;

        private static void Main(string[] args)
        {
            Log.Logger = SerilogLoggerFactory.CreateSerilogLogger();
            try
            {
                Log.Information("Configure web host ({ApplicationContext})...", AppName);
                IHost host = CreateHostBuilder(args).Build();

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationCOntext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string configurationType = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType);

                    if (string.IsNullOrEmpty(configurationType) || !ConfigurationTypes.Configurations.Contains(configurationType))
                    {
                        throw ExceptionMessages.MissingConfigurationTypeException;
                    }

                    IMayhemConfigurationService mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), configurationType);

                    services.AddLogging(c => c.AddSerilog());
                    services.AddHostedService<ItemTokenTransferBackgroundWorker>();
                    services.AddItemQueuePublisher(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueItemConnectionString);

                    services.AddSingleton<IWorkerService, WorkerService>();
                    services.AddScoped<IBlockRepository, BlockRepository>();
                    services.AddScoped<IWeb3>(x => new Web3(mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.Web3ProviderEndpoint));
                    services.AddScoped<IBlockchainService, BlockchainService>();
                });
        }
    }
}
