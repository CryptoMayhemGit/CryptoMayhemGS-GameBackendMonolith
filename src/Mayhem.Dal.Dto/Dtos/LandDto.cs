using Mayhem.Dal.Dto.Dtos.Base;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class LandDto : TableBaseDto
    {
        public long Id { get; set; }
        public LandsType LandTypeId { get; set; }
        public int LandInstanceId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsMinted { get; set; }

        public ICollection<UserLandDto> UserLands { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LandDto dto &&
                   Id == dto.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
