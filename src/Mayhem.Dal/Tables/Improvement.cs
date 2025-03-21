using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;
using Mayhem.Dal.Tables.Nfts;

namespace Mayhem.Dal.Tables
{
    public class Improvement : TableBase
    {
        public long Id { get; set; }
        public long LandId { get; set; }
        public ImprovementsType ImprovementTypeId { get; set; }

        public Land Land { get; set; }
        public ImprovementType ImprovementType { get; set; }
    }
}
