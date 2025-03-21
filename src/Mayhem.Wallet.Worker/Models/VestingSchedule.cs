using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace Mayhem.Wallet.Worker.Models
{
    [Function("VestingSchedule")]
    public class VestingSchedule
    {
        [Parameter("address", "beneficiary", 2)]
        public string beneficiary { get; set; }
        [Parameter("uint256", "cliff", 3)]
        public BigInteger cliff { get; set; }
        [Parameter("uint256", "amountOnStart", 4)]
        public BigInteger amountOnStart { get; set; }

        [Parameter("uint256", "amountOnStartReleased", 5)]
        public BigInteger amountOnStartReleased { get; set; }

        [Parameter("uint256", "start", 6)]
        public BigInteger start { get; set; }

        [Parameter("uint256", "duration", 7)]
        public BigInteger duration { get; set; }

        [Parameter("uint256", "baseAmount", 8)]
        public BigInteger baseAmount { get; set; }

        [Parameter("uint256", "baseAmountReleased", 9)]
        public BigInteger baseAmountReleased { get; set; }

        [Parameter("uint256", "amountTotal", 10)]
        public BigInteger amountTotal { get; set; }

        [Parameter("uint256", "released", 11)]
        public BigInteger released { get; set; }

    }
}
