using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Mayhem.Consumer.Dal.Repositories;
using Mayhem.Item.QueueConsumer;
using Mayhem.Logger;
using Mayhem.Queue.Classes;
using Mayhem.Queue.Consumer.Base.Interfaces;
using Mayhem.Queue.Consumer.Base.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Mayhem.Avatar.Worker.App
{
    internal class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;
        private static readonly string AppName = Namespace;

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

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string configurationType = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType);

                    if (string.IsNullOrEmpty(configurationType) || !ConfigurationTypes.Configurations.Contains(configurationType))
                    {
                        throw new Exception("Missing configuration type! Need to add ");
                    }

                    IMayhemConfigurationService mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), configurationType);

                    services.AddLogging(c => c.AddSerilog());
                    services.AddTransient<IQueueConsumer<ItemQueueConsumer>, ItemQueueConsumer>();
                    services.AddHostedService<AzureQueueConsumerBackgroundService<ItemQueueConsumer>>();
                    services.AddSingleton<IItemQueueClient>(new ItemQueueClient(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueItemConnectionString, QueueNames.ItemQueue));
                    services.AddScoped<IItemRepository, ItemRepository>();
                    services.AddScoped<IDapperWrapper, DapperWrapper>();
                });
        }
    }
}
