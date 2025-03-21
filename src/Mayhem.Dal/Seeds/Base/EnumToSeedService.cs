using Mayhem.Dal.Tables.Base;
using System;
using System.Collections.Generic;

namespace Mayhem.Dal.Seeds.Base
{
    public static class EnumToSeedService
    {
        /// <summary>
        /// Creates seed data from enum values, used to create dictionary tables in sql       
        /// </summary>
        public static IEnumerable<T> GetSeedData<T, E>()
            where E : Enum
            where T : DictionaryTableBase<E>, new()
        {
            List<T> data = new();

            foreach (object item in Enum.GetValues(typeof(E)))
            {
                T dataItem = new()
                {
                    Id = (E)(object)item.GetHashCode(),
                    Name = item.ToString()
                };

                data.Add(dataItem);
            }

            return data;
        }
    }
}
