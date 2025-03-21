using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Commands.GetUser
{
    public class GetUserCommandResponseDto
    {
        public GameUserDto GameUser { get; set; }
        public ICollection<UserResourceDto> UserResources { get; set; }
        public ICollection<NpcDto> Npcs { get; set; }
        public ICollection<UserLandDto> UserLands { get; set; }
        public ICollection<ItemDto> Items { get; set; }
    }
}
