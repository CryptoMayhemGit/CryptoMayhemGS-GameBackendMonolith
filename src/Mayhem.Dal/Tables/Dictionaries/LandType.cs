﻿using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables.Dictionaries
{
    public class LandType : DictionaryTableBase<LandsType>
    {
        public ICollection<Land> Lands { get; set; }
    }
}
