using AutoMapper;
using Mayhem.BlobStorage.Extensions;
using Mayhem.Blockchain.Extensions;
using Mayhem.Cache.Extensions;
using Mayhem.Common.Services.Extensions;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Behaviours;
using Mayhem.Dal.Contexts;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Mappings;
using Mayhem.Dal.Tables;
using Mayhem.HttpClient.Extensions;
using Mayhem.Queue.Publisher.Extensions;
using Mayhem.Settings;
using Mayhem.Setup;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Mappings.Dtos;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mayhem.ApplicationSetup
{
    public static class ApplicationConfigurationExtensions
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(NftStandardModelMapping), typeof(TableDtoMappings));
        }

        public static void AddMayhemContext(this IServiceCollection services, IMayhemConfigurationService mayhemConfigurationService)
        {
            string connectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;

            services
                .AddDbContext<MayhemDataContext>
                (
                    options => options.UseSqlServer(connectionString)
                );

            services.AddScoped<IMayhemDataContext, MayhemDataContext>();
        }

        public static void AddServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddMediator();
            services.AddRepositories();
            services.AddRestServices();
            services.AddPipelines();
            services.AddNotificationQueuePublisher(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueNotificationConnectionString);
            services.AddBlockchain(mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.Web3ProviderEndpoint);
            services.AddApplicationInsightWithDefaultProcessor(mayhemConfiguration);
            services.AddAutoMapperConfiguration();
            services.AddMayhemContext(mayhemConfiguration);
            services.AddCustomHttpClient();
            services.AddCache(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.CacheConnectionString, mayhemConfiguration.MayhemConfiguration.CommonConfiguration.CacheName);
            services.AddBlobStorage(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureBlobStorageConnectionString);
            services.AddCommonServices();
            services.AddSettings(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString);
        }

        public static void AddServicesForUnitTests(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            services.AddMediator();
            services.AddNotificationQueuePublisher(mayhemConfiguration.MayhemConfiguration.ConnectionStringsConfigruation.AzureQueueNotificationConnectionString);
            services.AddBlockchain(mayhemConfiguration.MayhemConfiguration.ServiceDiscoveryConfigruation.Web3ProviderEndpoint);
            services.AddRepositories();
            services.AddRestServices();
            services.AddPipelines();
            services.AddAutoMapperConfiguration();
        }

        public static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(LoginCommandRequest).Assembly);
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IUserRepository>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository"))).AsMatchingInterface());
        }

        public static void AddRestServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IAuthService>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))).AsMatchingInterface());
        }

        public static void AddPipelines(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        public static void AddJwtIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MayhemDataContext>()
                .AddDefaultTokenProviders();
        }
    }
}
