using AutoMapper;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Contexts;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Mappings;
using Mayhem.Land.Bl.Interfaces;
using Mayhem.Land.Bl.Mappings;
using Mayhem.Land.Worker;
using Mayhem.Package.Bl.Interfaces;
using Mayhem.Package.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mayhem.Generator.ApplicationSetup
{
    public static class ApplicationConfigurationExtensions
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TableDtoMappings), typeof(LandMapping));
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

        public static void AddServicesForUnitTests(this IServiceCollection services)
        {
            services.AddNpcBlServices();
            services.AddLandBlServices();
            services.AddItemBlServices();
            services.AddPackageBlServices();
            services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            services.AddRepositories();
            services.AddAutoMapperConfiguration();
        }

        public static void AddNpcServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddNpcBlServices();
            services.AddServices(mayhemConfiguration);
        }

        public static void AddLandServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddLandBlServices();
            services.AddLandBackgroundServices();

            services.AddServices(mayhemConfiguration);
        }

        public static void AddPackageServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddPackageBlServices();
            services.AddPackageBackgroundServices();

            services.AddServices(mayhemConfiguration);
            services.AddItemBlServices();
            services.AddNpcBlServices();
        }

        public static void AddItemServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddItemBlServices();
            services.AddServices(mayhemConfiguration);
        }

        private static void AddServices(this IServiceCollection services, IMayhemConfigurationService mayhemConfiguration)
        {
            services.AddRepositories();
            services.AddAutoMapperConfiguration();
            services.AddMayhemContext(mayhemConfiguration);
        }

        private static void AddLandBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<LandGeneratorBackgroundService>();
        }
        private static void AddPackageBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<PackageGeneratorBackgroundService>();
        }

        private static void AddNpcBlServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<INpcGeneratorService>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))).AsMatchingInterface());
        }


        private static void AddLandBlServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<ILandGeneratorService>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))).AsMatchingInterface());
        }
        private static void AddPackageBlServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IPackageGeneratorService>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))).AsMatchingInterface());
        }
        private static void AddItemBlServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IItemGeneratorService>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))).AsMatchingInterface());
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IUserRepository>().AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository"))).AsMatchingInterface());
        }
    }
}
