using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PIIIProject.Models
{
    public class Player : Creature, IMapObject, ICollidable
    {
        public const string PLAYER_DEFAULT_NAME = "Player";
        public const char PLAYER_DISPLAY_CHAR = '@';
        private const int STARTING_HEALTH = 25, STARTING_STRENGTH = 10, STARTING_DEFENSE = 1;

        private int _currentX, _currentY;
        private ObservableCollection<Item> _inventory;

        public ObservableCollection<Item> Inventory
        {
            get { return _inventory; }
            set
            {
                if (value is null)
                    throw new ArgumentNullException("The item cannot be null.");
                _inventory = value;
            }
        }
        public int CurrentX
        {
            get { return _currentX; }
        }
        public int CurrentY
        {
            get { return _currentY; }
        }

        public Player(GameMap map, int spawnX, int spawnY) : base(PLAYER_DEFAULT_NAME, STARTING_HEALTH, STARTING_STRENGTH, STARTING_DEFENSE)
        {
            _currentX = spawnX;
            _currentY = spawnY;
            _inventory = new ObservableCollection<Item>();

            map.AddThing(this, spawnX, spawnY);
        } 

        public char GetMapDisplayChar()
        { 
            return PLAYER_DISPLAY_CHAR; 
        }

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
            {
                if (thing is ICollidable)
                    return;
                else if (thing is Enemy)
                {
                    Views.Combat combatWindow = new Views.Combat(this, thing as Enemy);
                    combatWindow.Show();
                }
            }

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
