namespace Mayhem.Dal.Dto.Commands.GetUser
{
    public class GetUserCommandRequestDto
    {
        public int UserId { get; set; }
        public bool WithResources { get; set; }
        public bool WithItems { get; set; }
        public bool WithNpcs { get; set; }
        public bool WithLands { get; set; }
    }
}
