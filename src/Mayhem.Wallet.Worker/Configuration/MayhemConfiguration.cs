using System;
using System.IO;

namespace Mayhem.Wallet.Worker.Configuration
{
    public class MayhemConfiguration
    {
        public const string Web3Provider = "https://bsc-dataseed.binance.org/";
        public const string AdriaVestingAddress = "0xBbD93569C664ce6FBA65B4B7f36BA93B1E8C7a86";
        public const string Divider = "1000000000000000000";
        public const string AdriaTokenPerOneUsdc = "625";
        public static string AdriaVestingAbi;

        static MayhemConfiguration()
        {
            string home = Environment.GetEnvironmentVariable("HOME");
            string wwwroot = Path.Combine(home, "site", "wwwroot");

            AdriaVestingAbi = File.ReadAllText(Path.Combine(wwwroot, "Configuration", "abi.txt"));
        }
    }
}
