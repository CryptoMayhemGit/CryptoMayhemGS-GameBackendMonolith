namespace Mayhem.IntegrationTest.Base.Models
{
    public class SendActivationNotificationTestDto
    {
        public string Email { get; set; }
        public string Wallet { get; set; }
        public string SignedMessage { get; set; }
        public string MessageToSign { get; set; }
        public long Nonce { get; set; }
    }
}
