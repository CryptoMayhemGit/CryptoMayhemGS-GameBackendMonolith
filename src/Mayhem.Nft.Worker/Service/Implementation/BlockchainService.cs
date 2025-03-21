using Mayhem.Nft.Worker.Configuration;
using Mayhem.Nft.Worker.Service.Interface;
using Nethereum.Contracts;
using Nethereum.Web3;
using System.Threading.Tasks;

namespace Mayhem.Nft.Worker.Service.Implementation
{
    public class BlockchainService : IBlockchainService
    {
        private Contract? contract;
        private Function ownerOfFunction;

        public BlockchainService(IWeb3 web3)
        {
            this.contract = web3.Eth.GetContract(MayhemConfiguration.TGLPCryptoMayhemNFTAbi, MayhemConfiguration.TGLPCryptoMayhemNFT);
            this.ownerOfFunction = contract.GetFunction("ownerOf");
        }

        public async Task<string> GetOwnerOf(int tokenId)
        {
            return await this.ownerOfFunction.CallAsync<string>(tokenId);
        }
    }
}
