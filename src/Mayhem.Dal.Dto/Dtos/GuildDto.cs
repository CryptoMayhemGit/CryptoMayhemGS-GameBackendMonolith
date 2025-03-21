using Mayhem.Dal.Dto.Dtos.Base;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GuildDto : TableBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }

        public ICollection<GuildResourceDto> GuildResources { get; set; }
        public ICollection<GameUserDto> Users { get; set; }
    }
}
