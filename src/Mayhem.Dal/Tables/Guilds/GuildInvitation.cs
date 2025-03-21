using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;

namespace Mayhem.Dal.Tables.Guilds
{
    public class GuildInvitation : TableBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GuildId { get; set; }
        public GuildInvitationsType InvitationType { get; set; }

        public GameUser User { get; set; }
        public Guild Guild { get; set; }
    }
}
