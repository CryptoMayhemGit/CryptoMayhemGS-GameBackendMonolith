using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Land.Bl.Dtos
{
    public class ImportLandDto
    {
        public LandsType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
