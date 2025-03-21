using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GuildInvitationDto : TableBaseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GuildId { get; set; }
        public GuildInvitationsType InvitationType { get; set; }

        public GameUserDto User { get; set; }
        public GuildDto Guild { get; set; }
    }
}
