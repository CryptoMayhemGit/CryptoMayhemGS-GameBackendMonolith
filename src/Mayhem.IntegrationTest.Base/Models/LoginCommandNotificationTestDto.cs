namespace Mayhem.IntegrationTest.Base.Models
{
    public class LoginCommandNotificationTestDto
    {
        public string Wallet { get; set; }
        public string SignedMessage { get; set; }
        public string MessageToSign { get; set; }
        public long Nonce { get; set; }
    }
}
