using System;

namespace Mayhem.Dal.Interfaces.Base
{
    /// <summary>
    /// Is used as a contract with entity framework, albo adds two columns to all sql tables
    /// </summary>
    public interface ITableBase
    {
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        DateTime? CreationDate { get; set; }
        /// <summary>
        /// Gets or sets the last modification date.
        /// </summary>
        /// <value>
        /// The last modification date.
        /// </value>
        DateTime? LastModificationDate { get; set; }
    }
}
