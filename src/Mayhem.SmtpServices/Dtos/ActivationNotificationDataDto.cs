using System;

namespace Mayhem.SmtpServices.Dtos
{
    public class ActivationNotificationDataDto
    {
        public string Wallet { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
