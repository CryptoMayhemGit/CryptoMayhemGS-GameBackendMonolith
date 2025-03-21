using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.Test.Common
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Used in tests to add resources to the player      
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static ICollection<UserResource> GetBasicUserResourcesWithValue(int defaultValue = 1000)
        {
            return Enum.GetValues(typeof(ResourcesType)).Cast<ResourcesType>().Select(type => new UserResource()
            {
                ResourceTypeId = type,
                Value = defaultValue,
            }).ToList();
        }

        /// <summary>
        /// Used in tests to add resources to the guild        
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static ICollection<GuildResource> GetBasicGuildResourcesWithValue(int defaultValue = 1000000)
        {
            return Enum.GetValues(typeof(ResourcesType)).Cast<ResourcesType>().Select(type => new GuildResource()
            {
                ResourceTypeId = type,
                Value = defaultValue,
            }).ToList();
        }
    }
}
