using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using System;
using System.Linq.Expressions;

namespace Mayhem.Dal.Helpers
{
    public static class EntityHelper
    {
        /// <summary>
        /// Gets adjacent hexagonal squares for user land position on hex map    
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        public static Expression<Func<UserLand, bool>> GetNeighboursByUserLand(int userId, int positionX, int positionY)
        {
            return x => x.UserId == userId &&
                                (positionY % 2 == 1 &&
                                ((x.Land.PositionX == positionX && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY + 1) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY + 1))
                                || (positionY % 2 == 0 &&
                                ((x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY + 1) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY + 1))));
        }

        /// <summary>
        /// Gets adjacent hexagonal squares for npc position on hex map 
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="positionX">The position x.</param>
        /// <param name="positionY">The position y.</param>
        public static Expression<Func<Npc, bool>> GetNeighboursByNpc(int userId, int positionX, int positionY)
        {
            return x => x.UserId == userId &&
                                (positionY % 2 == 1 &&
                                ((x.Land.PositionX == positionX && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY + 1) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY + 1))
                                || (positionY % 2 == 0 &&
                                ((x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY - 1) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX + 1 && x.Land.PositionY == positionY) ||
                                (x.Land.PositionX == positionX - 1 && x.Land.PositionY == positionY + 1) ||
                                (x.Land.PositionX == positionX && x.Land.PositionY == positionY + 1))));
        }
    }
}
