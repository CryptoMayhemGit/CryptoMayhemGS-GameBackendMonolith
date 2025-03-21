namespace Mayhem.Worker.Dal.Dto
{
    public class UserLandNpcDto
    {
        public int? NpcUserId { get; set; }
        public int LandUserId { get; set; }
        public bool Owned { get; set; }
    }
}
