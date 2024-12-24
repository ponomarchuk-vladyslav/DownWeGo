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
        // Constants, default stats for the player.
        public const string PLAYER_DEFAULT_NAME = "Player";
        public const char PLAYER_DISPLAY_CHAR = '@';
        private const int STARTING_HEALTH = 25, STARTING_STRENGTH = 10, STARTING_DEFENSE = 0, STARTING_LEVEL = 1;

        // Backing field for the inventory. ObservableCollection makes it easier to display.
        private ObservableCollection<Item> _inventory;

        /// <summary>
        /// A property that allows to get and set the inventory.
        /// </summary>
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

        /// <summary>
        /// Private constructor used to create a new player. Initializes the inventory (empty).
        /// </summary>
        /// <param name="spawnX">The starting x coordinate of the player.</param>
        /// <param name="spawnY">The starting y coordinate of the player.</param>
        private Player(int spawnX, int spawnY) : base(spawnX, spawnY, PLAYER_DEFAULT_NAME, STARTING_LEVEL, STARTING_HEALTH, STARTING_STRENGTH, STARTING_DEFENSE)
        {
            _inventory = new ObservableCollection<Item>();
        }

        /// <summary>
        /// Private constructor used to create a new player when loading player data from save.
        /// </summary>
        /// <param name="spawnX">X Coord of the player.</param>
        /// <param name="spawnY">Y Coord of the player.</param>
        /// <param name="name">Name of the player.</param>
        /// <param name="level">Level of the player.</param>
        /// <param name="health">Health of the player.</param>
        /// <param name="strength">Strength of the player.</param>
        /// <param name="defense">Defense of the player.</param>
        /// <param name="blockMult">Block multiplier of the player.</param>
        private Player(int spawnX, int spawnY, string name, int level, int health, int strength, int defense, double blockMult) : base(spawnX, spawnY, name, level, health, strength, defense, blockMult)
        {

        }

        /// <summary>
        /// Gets the character representing the player.
        /// </summary>
        /// <returns>A character.</returns>
        public char GetMapDisplayChar()
        { 
            return PLAYER_DISPLAY_CHAR; 
        }

        /// <summary>
        /// Moves the player and checks for collision with anything. Picks up any items.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <param name="map">The map on which to move the player.</param>
        /// <returns>An object with which the player has collided. If the player hasn't collided, returns null.</returns>
        public ICollidable MovePlayer(GameMap.Direction direction, GameMap map)
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

            // Checks if the player tries to go outside the map
            if (nextY < 0 || nextY >= map.LogicMap.GetLength(0) || nextX < 0 || nextX >= map.LogicMap.GetLength(1))
                return null;

            // Checks if the player has collided with anything
            foreach (IMapObject thing in map.LogicMap[nextY, nextX])
            {
                if (thing is ICollidable)
                    return thing as ICollidable;
            }

            // Moves the player and updates his coords.
            map.MoveThing(this, CurrentX, CurrentY, direction);
            CurrentX = nextX;
            CurrentY = nextY;

            // Picks up any items in the cell the player has moved to.
            for (int i = map.LogicMap[CurrentY, CurrentX].Count - 1; i >= 0; i--)
            {
                if (map.LogicMap[CurrentY, CurrentX][i] is Item)
                {
                    Inventory.Add(map.LogicMap[CurrentY, CurrentX][i] as Item);
                    map.RemoveThing(map.LogicMap[CurrentY, CurrentX][i], CurrentX, CurrentY);
                }
            }

            return null;
        }

        /// <summary>
        /// Exports all the data of the player as a string of values divided by a divider character. Excludes the inventory.
        /// </summary>
        /// <returns>All the player data as a string.</returns>
        public string ExportSaveDataAsString()
        {
            char div = IMapObject.EXPORT_DIVIDER_CHAR;
            return $"{CurrentX}{div}{CurrentY}{div}{Name}{div}{Level}{div}{Health}{div}{Strength}{div}{Defense}{div}{BlockMultiplier}";
        }

        /// <summary>
        /// Creates a new player from the provided save data. Splits the string and tries to convert the values, then calls a constructor.
        /// </summary>
        /// <param name="saveDataString">The save data of the player.</param>
        /// <returns>A player object. If an exception occurs during conversion, returns null.</returns>
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

        /// <summary>
        /// Creates a new player and adds it to the map, at the corresponding coordinates. Made to mitigate the problem that the player coordinates and actual location on the map can be different.
        /// </summary>
        /// <param name="map">The map to which the player is to be added.</param>
        /// <param name="spawnX">The starting x coordinate of the player.</param>
        /// <param name="spawnY">The starting y coordinate of the player.</param>
        /// <returns>The player object created.</returns>
        public static Player NewPlayer(GameMap map, int spawnX, int spawnY)
        {
            Player player = new Player(spawnX, spawnY);

            map.AddThing(player, spawnX, spawnY);

            return player;
        }
    }
}
