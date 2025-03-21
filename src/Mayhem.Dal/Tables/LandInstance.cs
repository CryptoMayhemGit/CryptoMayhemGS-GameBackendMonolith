using Mayhem.Dal.Tables.Base;
using Mayhem.Dal.Tables.Nfts;
using System.Collections.Generic;

namespace Mayhem.Dal.Tables
{
    public class LandInstance : TableBase
    {
        public int Id { get; set; }

        public ICollection<Land> Lands { get; set; }
    }
}
