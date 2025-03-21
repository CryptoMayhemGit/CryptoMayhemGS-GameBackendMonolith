using Mayhem.Wallet.Worker.Configuration;
using Mayhem.Wallet.Worker.Models;
using Mayhem.Wallet.Worker.Service.Interface;
using Nethereum.Contracts;
using Nethereum.Web3;
using System.Threading.Tasks;

namespace Mayhem.Wallet.Worker.Service.Implementation
{
    public class BlockchainService : IBlockchainService
    {
        private Contract? web3Contract;
        Function getVestingSchedulesCountFunction;
        Function getVestingIdAtIndexFunction;
        Function getVestingScheduleFunction;

        public BlockchainService(IWeb3 web3)
        {
            Init(web3);
        }

        private void Init(IWeb3 web3)
        {
            web3Contract = web3.Eth.GetContract(MayhemConfiguration.AdriaVestingAbi, MayhemConfiguration.AdriaVestingAddress);
            getVestingSchedulesCountFunction = web3Contract.GetFunction("getVestingSchedulesCount");
            getVestingIdAtIndexFunction = web3Contract.GetFunction("getVestingIdAtIndex");
            getVestingScheduleFunction = web3Contract.GetFunction("getVestingSchedule");
        }

        public async Task<int> GetVestingSchedulesCountFunctionAsync()
        {
            return await getVestingSchedulesCountFunction.CallAsync<int>();
        }

        public async Task<byte[]> GetVestingIdAtIndexFunctionAsync(long index)
        {
            return await getVestingIdAtIndexFunction.CallAsync<byte[]>(index);
        }

        public async Task<VestingSchedule> GetVestingScheduleFunctionAsync(byte[] details)
        {
            return await getVestingScheduleFunction.CallAsync<VestingSchedule>(details);
        }
    }
}
