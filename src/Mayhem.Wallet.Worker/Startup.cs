using Mayhem.Configuration.Classes;
using Mayhem.Messages;
using Mayhem.Wallet.Worker.Configuration;
using Mayhem.Wallet.Worker.Service.Implementation;
using Mayhem.Wallet.Worker.Service.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Web3;
using System;

[assembly: FunctionsStartup(typeof(Mayhem.Wallet.Worker.Startup))]
namespace Mayhem.Wallet.Worker
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string configurationType = Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType);

            if (string.IsNullOrEmpty(configurationType) || !ConfigurationTypes.Configurations.Contains(configurationType))
            {
                throw ExceptionMessages.MissingConfigurationTypeException;
            }

            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<IBlockRepository, BlockRepository>();
            builder.Services.AddScoped<IGameUserRepository, GameUserRepository>();
            builder.Services.AddScoped<IWeb3>(x => new Web3(MayhemConfiguration.Web3Provider));
            builder.Services.AddScoped<IBlockchainService, BlockchainService>();
        }
    }
}
