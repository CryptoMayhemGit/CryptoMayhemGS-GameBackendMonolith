using Mayhem.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Mayhem.WebApi
{
    public class Program
    {
        public static readonly string AppName = typeof(Program).Namespace;

        public static void Main(string[] args)
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
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
