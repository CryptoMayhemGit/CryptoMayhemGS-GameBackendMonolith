using System;

namespace Mayhem.Dal.Tables.Base
{
    //   
    /// <summary>
    /// Adds two default columns to the dictionary table  
    /// </summary>
    public class DictionaryTableBase<T> : TableBase
        where T : Enum
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
