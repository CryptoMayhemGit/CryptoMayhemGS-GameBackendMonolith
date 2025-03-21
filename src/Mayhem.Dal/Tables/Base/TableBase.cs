using Mayhem.Dal.Interfaces.Base;
using System;

namespace Mayhem.Dal.Tables.Base
{
    /// <summary>
    /// Adds two default columns to the table    
    /// </summary>
    public class TableBase : ITableBase
    {
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
    }
}
