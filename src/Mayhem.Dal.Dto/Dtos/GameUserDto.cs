using Mayhem.Dal.Dto.Dtos.Base;
using System;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GameUserDto : TableBaseDto
    {
        public int Id { get; set; }
        public string WalletAddress { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? GuildId { get; set; }
    }
}
