namespace Mayhem.Dal.Dto.Dtos
{
    public class AddUserToGuildDto
    {
        public GuildDto Guild { get; set; }
        public GameUserDto User { get; set; }
    }
}
