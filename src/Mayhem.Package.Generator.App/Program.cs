using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Generator.ApplicationSetup;
using Mayhem.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Mayhem.Package.Generator.App
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
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
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
                        throw new Exception("Missing configuration type! Need to add enviroment variable");
                    }

                    IMayhemConfigurationService mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), configurationType);

                    services.AddLogging(c => c.AddSerilog());
                    services.AddPackageServices(mayhemConfiguration);
                });
        }
    }
}
