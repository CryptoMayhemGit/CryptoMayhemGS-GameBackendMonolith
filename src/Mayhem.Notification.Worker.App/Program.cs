﻿using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Logger;
using Mayhem.Messages;
using Mayhem.Queue.Classes;
using Mayhem.Queue.Publisher;
using Mayhem.SmtpBase.Services.Implementations;
using Mayhem.SmtpBase.Services.Interfaces;
using Mayhem.SmtpServices.Interfaces;
using Mayhem.SmtpServices.Services;
using Mayhem.Worker.Notification;
using Mayhem.Worker.Notification.Interfaces;
using Mayhem.Worker.Notification.Services;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Workers.Dal.Repositories.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Mayhem.Notification.Worker.App
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
                    services.AddHostedService<NotificationBackgroundWorker>();
                    NotificationQueuePublisher notificationQueuePublisher = new(new QueueConfiguration() { QueueName = QueueNames.NotificationQueue, ServiceBusConnectionString = mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueNotificationConnectionString });
                    services.AddSingleton<INotificationQueuePublisher>(notificationQueuePublisher);

                    services.AddSingleton<IWorkerService, WorkerService>();
                    services.AddScoped<IWarningNotificationService, WarningNotificationService>(); 
                    services.AddScoped<INotificationRepository, NotificationRepository>(); 
                    services.AddScoped<ISmtpService, SmtpService>();
                });
        }
    }
}
