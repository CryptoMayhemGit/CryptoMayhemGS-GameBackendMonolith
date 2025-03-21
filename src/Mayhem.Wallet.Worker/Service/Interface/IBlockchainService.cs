using Mayhem.Wallet.Worker.Models;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Interface
{
    public interface IBlockchainService
    {
        Task<byte[]> GetVestingIdAtIndexFunctionAsync(long index);
        Task<VestingSchedule> GetVestingScheduleFunctionAsync(byte[] details);
        Task<int> GetVestingSchedulesCountFunctionAsync();
    }
}
