using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject
{
    internal class Player : IMapObject, ICollidable
    {
        public const char PLAYER_DISPLAY_CHAR = '@';

        private int CurrentX, CurrentY;

        public Player(Map map, int spawnX, int spawnY)
        {
            CurrentX = spawnX;
            CurrentY = spawnY;

            map.AddThing(this, spawnX, spawnY);
        } 

        public char GetMapDisplayChar()
        { return PLAYER_DISPLAY_CHAR; }

        public void MovePlayer(Map.Direction direction, Map map)
        {
            int nextX = CurrentX, nextY = CurrentY;

            switch (direction)
            {
                case Map.Direction.Up:
                    nextY++;
                    break;
                case Map.Direction.Down:
                    nextY--;
                    break;
                case Map.Direction.Left:
                    nextX--;
                    break;
                case Map.Direction.Right:
                    nextX++;
                    break;
                default:
                    break;
            }
            // Temp, to replace with a method
            if (map.LogicMap[nextY, nextX].OfType<ICollidable>().Any())
                return;

            map.MoveThing(this, CurrentX, CurrentY, direction);
        }
    }
}
