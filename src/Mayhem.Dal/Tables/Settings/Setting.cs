using Mayhem.Dal.Tables.Base;
using System.ComponentModel.DataAnnotations;

namespace Mayhem.Dal.Tables.Settings
{
    public class Setting : TableBase
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
