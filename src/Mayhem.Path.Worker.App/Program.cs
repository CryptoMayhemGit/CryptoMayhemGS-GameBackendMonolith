using Mayhem.Common.Services.PathFindingService.Implementations;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Logger;
using Mayhem.Messages;
using Mayhem.Worker.Path;
using Mayhem.Worker.Path.Interfaces;
using Mayhem.Worker.Path.Services;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Workers.Dal.Repositories.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Mayhem.Path.Worker.App
{
    internal class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace;

        static void Main(string[] args)
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
                    services.AddHostedService<PathBackgroundWorker>();

                    services.AddSingleton<IPathWorkerService, PathWorkerService>();
                    services.AddScoped<ILandRepository, LandRepository>();
                    services.AddScoped<INpcRepository, NpcRepository>();
                    services.AddScoped<ITravelRepository, TravelRepository>();
                    services.AddScoped<INpcRepository, NpcRepository>();
                    services.AddScoped<IUserLandRepository, UserLandRepository>();
                    services.AddScoped<IPathFindingService, PathFindingService>();
                });
        }
    }
}
