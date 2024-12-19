using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Player : IMapObject, ICollidable
    {
        public const char PLAYER_DISPLAY_CHAR = '@';

        private int CurrentX, CurrentY;

        public Player(GameMap map, int spawnX, int spawnY)
        {
            CurrentX = spawnX;
            CurrentY = spawnY;

            map.AddThing(this, spawnX, spawnY);
        } 

        public char GetMapDisplayChar()
        { return PLAYER_DISPLAY_CHAR; }

        public void MovePlayer(GameMap.Direction direction, GameMap map)
        {
            int nextX = CurrentX, nextY = CurrentY;

            switch (direction)
            {
                case GameMap.Direction.Up:
                    nextY++;
                    break;
                case GameMap.Direction.Down:
                    nextY--;
                    break;
                case GameMap.Direction.Left:
                    nextX--;
                    break;
                case GameMap.Direction.Right:
                    nextX++;
                    break;
                default:
                    break;
            }

            if (nextY < 0 || nextY >= map.LogicMap.GetLength(0) || nextX < 0 || nextX >= map.LogicMap.GetLength(1))
                return;

            // Temp, to replace with a method
            if (map.LogicMap[nextY, nextX].OfType<ICollidable>().Any())
                return;

            map.MoveThing(this, CurrentX, CurrentY, direction);
            CurrentX = nextX;
            CurrentY = nextY;
        }
    }
}
