using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Dictionaries;

namespace Mayhem.Dal.Tables
{
    public class UserResource : TableBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ResourcesType ResourceTypeId { get; set; }
        public double Value { get; set; }

        public ResourceType ResourceType { get; set; }
        public GameUser GameUser { get; set; }
    }
}
