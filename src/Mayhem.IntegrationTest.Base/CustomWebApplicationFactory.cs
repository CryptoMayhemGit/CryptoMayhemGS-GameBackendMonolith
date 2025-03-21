using Mayhem.Cache.Interfaces;
using Mayhem.Cache.Mocks;
using Mayhem.Dal.Contexts;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Mock;
using Mayhem.WebApi;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Base
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string dbName;
        private IServiceProvider serviceProvider;

        public CustomWebApplicationFactory(string dbName)
        {
            this.dbName = dbName;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                MockNotificationPublisherService(services);

                ServiceDescriptor dbOptionsDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<MayhemDataContext>));
                List<ServiceDescriptor> contextDescriptor = services.Where(x => x.ServiceType == typeof(IMayhemDataContext)).ToList();
                ServiceDescriptor cacheDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(ICacheService));

                if (dbOptionsDescriptor != null)
                {
                    services.Remove(dbOptionsDescriptor);
                }

                foreach (ServiceDescriptor descriptor in contextDescriptor ?? Enumerable.Empty<ServiceDescriptor>())
                {
                    services.Remove(descriptor);
                }

                if (cacheDescriptor != null)
                {
                    services.Remove(cacheDescriptor);
                }

                services.AddSingleton<IMayhemDataContext>(new MayhemDataContextMock(dbName));
                services.AddSingleton<ICacheService>(new CacheServiceMock());

                serviceProvider = services.BuildServiceProvider();
            });
        }

        public static void MockNotificationPublisherService(IServiceCollection services)
        {
            Mock<INotificationPublisherService> notificationPublisherServiceMock = new();
            notificationPublisherServiceMock.Setup(p => p.PublishMessageAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            services.RemoveAll<INotificationPublisherService>();
            services.AddTransient(sp => notificationPublisherServiceMock.Object);
        }

        internal T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }
    }
}
