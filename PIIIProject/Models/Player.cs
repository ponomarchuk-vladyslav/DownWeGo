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
        private const int STARTING_HEALTH = 25;

        private int _currentX, _currentY;
        private int _health;
        private List<Item> _inventory;

        public int Health
        {
            get { return _health; }
            set
            {
                if (value < 0)
                    throw new Exception("Health can't be negative. Can't be more dead than dead...");

                _health = value;
            }
        }

        public List<Item> Inventory
        {
            get { return _inventory; }
            set
            {
                if (value is null)
                    throw new ArgumentNullException("The item cannot be null.");
                _inventory = value;
            }
        }

        public Player(GameMap map, int spawnX, int spawnY)
        {
            _currentX = spawnX;
            _currentY = spawnY;
            _health = STARTING_HEALTH;
            _inventory = new List<Item>();

            map.AddThing(this, spawnX, spawnY);
        } 

        public char GetMapDisplayChar()
        { return PLAYER_DISPLAY_CHAR; }

        public void MovePlayer(GameMap.Direction direction, GameMap map)
        {
            int nextX = _currentX, nextY = _currentY;

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

            foreach (IMapObject thing in map.LogicMap[nextY, nextX])
                if (thing is ICollidable)
                    return;

            map.MoveThing(this, _currentX, _currentY, direction);
            _currentX = nextX;
            _currentY = nextY;

            for (int i = map.LogicMap[_currentY, _currentX].Count - 1; i >= 0; i--)
            {
                if (map.LogicMap[_currentY, _currentX][i] is Item)
                {
                    Inventory.Add(map.LogicMap[_currentY, _currentX][i] as Item);
                    map.RemoveThing(map.LogicMap[_currentY, _currentX][i], _currentX, _currentY);
                }
            }
        }
    }
}
