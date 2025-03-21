using Mayhem.Nft.Worker.Configuration;
using Mayhem.Nft.Worker.Repository.Implementation;
using Mayhem.Nft.Worker.Repository.Interface;
using Mayhem.Nft.Worker.Service.Implementation;
using Mayhem.Nft.Worker.Service.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Web3;

[assembly: FunctionsStartup(typeof(Mayhem.Nft.Worker.Startup))]
namespace Mayhem.Nft.Worker
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IWeb3>(x => new Web3(MayhemConfiguration.Web3Provider));
            builder.Services.AddScoped<IBlockchainService, BlockchainService>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<IGameUserRepository, GameUserRepository>();
        }
    }
}
