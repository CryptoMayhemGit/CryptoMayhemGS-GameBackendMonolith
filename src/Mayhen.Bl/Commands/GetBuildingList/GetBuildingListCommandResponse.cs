using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetBuildingList
{
    public class GetBuildingListCommandResponse
    {
        public IEnumerable<BuildingsType> Buildings { get; set; }
    }
}
