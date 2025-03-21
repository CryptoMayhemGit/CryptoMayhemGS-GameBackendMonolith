using FluentValidation;
using Mayhem.ApplicationSetup;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Filters;
using Mayhem.HealthCheck;
using Mayhem.Messages;
using Mayhem.Setup;
using Mayhem.Swagger;
using Mayhem.WebApi.Dto.SwaggerExamples;
using Mayhem.WebApi.Filters;
using Mayhen.Bl.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mayhem.WebApi
{
    public class Startup
    {
        private const string ApiTitle = "Mayhem.WebApi";
        private const string CorsPolicy = nameof(CorsPolicy);

        public void ConfigureServices(IServiceCollection services)
        {
            string configurationType = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType);

            if (string.IsNullOrEmpty(configurationType) || !ConfigurationTypes.Configurations.Contains(configurationType))
            {
                throw ExceptionMessages.MissingConfigurationTypeException;
            }

            IMayhemConfigurationService mayhemConfiguration = services.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), configurationType);

            services.AddMvc(options =>
            {
                options.Filters.Add<TokenSliderFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ValidationFilter>();
            });

            AssemblyScanner.FindValidatorsInAssemblyContaining<AcceptInvitationByOwnerCommandRequestValidator>().ForEach(pair =>
            {
                services.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
                services.Add(ServiceDescriptor.Transient(pair.ValidatorType, pair.ValidatorType));
            });
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddServices(mayhemConfiguration);
            services.AddCustomHealthChecks(mayhemConfiguration.MayhemConfiguration);
            services.ConfigureSwaggerWithExamples<ExampleLoginCommandRequest>(
                ApiTitle,
                new List<string>()
                {
                    Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
                },
                true);

            services.AddJwtIdentity();
            services.AddJwtAuthentication(mayhemConfiguration);

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                .Build());
            });

            if (!mayhemConfiguration.MayhemConfiguration.CommonConfiguration.ApiAuthorizationEnabled)
            {
                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOwnSwagger(ApiTitle);

            app.UseRouting();

            app.UseCors(CorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //https://localhost:{port}/healthchecks-ui
                endpoints.MapCustomHealthChecks();
            });
        }
    }
}
