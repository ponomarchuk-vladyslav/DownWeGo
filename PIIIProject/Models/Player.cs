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
        private const int STARTING_HEALTH = 25, STARTING_STRENGTH = 10, STARTING_DEFENSE = 0, STARTING_LEVEL = 1;

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

        public Player(int spawnX, int spawnY) : base(spawnX, spawnY, PLAYER_DEFAULT_NAME, STARTING_LEVEL, STARTING_HEALTH, STARTING_STRENGTH, STARTING_DEFENSE)
        {
            _inventory = new ObservableCollection<Item>();
        }

        private Player(int spawnX, int spawnY, string name, int level, int health, int strength, int defense, double blockMult) : base(spawnX, spawnY, name, level, health, strength, defense, blockMult)
        {

        }

        public char GetMapDisplayChar()
        { 
            return PLAYER_DISPLAY_CHAR; 
        }

        public ICollidable MovePlayer(GameMap.Direction direction, GameMap map)
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
                return null;

            foreach (IMapObject thing in map.LogicMap[nextY, nextX])
            {
                if (thing is ICollidable)
                    return thing as ICollidable;
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

            return null;
        }

        public string ExportSaveDataAsString()
        {
            char div = IMapObject.EXPORT_DIVIDER_CHAR;
            return $"{CurrentX}{div}{CurrentY}{div}{Name}{div}{Level}{div}{Health}{div}{Strength}{div}{Defense}{div}{BlockMultiplier}";
        }

        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            try
            {
                string[] saveData = saveDataString.Split(IMapObject.EXPORT_DIVIDER_CHAR);

                int spawnX = int.Parse(saveData[0]);
                int spawnY = int.Parse(saveData[1]);
                string name = saveData[2];
                int level = int.Parse(saveData[3]);
                int health = int.Parse(saveData[4]);
                int strength = int.Parse(saveData[5]);
                int defense = int.Parse(saveData[6]);
                double blockMult = double.Parse(saveData[7]);

                return new Player(spawnX, spawnY, name, level, health, strength, defense, blockMult);
            }
            catch
            {
                return null;
            }
        }
    }
}
