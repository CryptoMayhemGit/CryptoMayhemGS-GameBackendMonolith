using System;
using System.IO;

namespace Mayhem.Nft.Worker.Configuration
{
    public class MayhemConfiguration
    {
        public const string Web3Provider = "https://bsc-dataseed.binance.org/";
        public const string TGLPCryptoMayhemNFT = "0x5C841C8D24D477e033ab80984cB5956976c0Ec13";
        public static string TGLPCryptoMayhemNFTAbi;
        static MayhemConfiguration()
        {
            string home = Environment.GetEnvironmentVariable("HOME");
            string wwwroot = Path.Combine(home, "site", "wwwroot");

            TGLPCryptoMayhemNFTAbi = File.ReadAllText(Path.Combine(wwwroot, "Configuration", "abi.txt"));
        }
    }
}
