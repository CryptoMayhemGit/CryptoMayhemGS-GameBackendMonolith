using System.Threading.Tasks;

namespace Mayhem.Nft.Worker.Service.Interface
{
    public interface IBlockchainService
    {
        Task<string> GetOwnerOf(int tokenId);
    }
}
