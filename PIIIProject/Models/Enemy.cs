using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Enemy : Creature, IMapObject, ICollidable
    {
        // The total number of enemies spawned
        private static int _enemyCount = 0;
        private bool _isBoss;

        /// <summary>
        /// The read-only property to get the total enemy count. Currently used to check if all enemies are killed before allowing the player to escape.
        /// </summary>
        public static int EnemyCount
        {
            get 
            { 
                return _enemyCount;
            }
        }

        // Constants
        public const string ENEMY_DEFAULT_NAME = "Skeleton";
        public const string BOSS_DEFAULT_NAME = "Abomination";
        public const char ENEMY_DISPLAY_CHAR = 'x';
        public const char BOSS_DISPLAY_CHAR = 'X';
        public const int BOSS_LEVEL_THRESHOLD = 10;
        public const int START_HEALTH = 10, START_STRENGTH = 2, START_DEFENSE = 1;

        /// <summary>
        /// Gets the character representing the enemy.
        /// </summary>
        /// <returns>A character.</returns>
        public char GetMapDisplayChar()
        {
            if (_isBoss)
                return BOSS_DISPLAY_CHAR;
            return ENEMY_DISPLAY_CHAR;  
        }

        /// <summary>
        /// Private constructor used for creating a new enemy. Checks if boss depending on level.
        /// </summary>
        /// <param name="spawnX">The starting X position of the enemy.</param>
        /// <param name="spawnY">The starting Y position of the enemy.</param>
        /// <param name="level">The starting level of the enemy. Affects stats.</param>
        /// <param name="name">The name of the enemy. If the argument is not provided or is empty, switches to default. If default and is boss, switches to default boss.</param>
        private Enemy(int spawnX, int spawnY, int level, string name) : base(spawnX, spawnY, name, level, START_HEALTH, START_STRENGTH, START_DEFENSE)
        {
            if (string.IsNullOrEmpty(name))
                name = ENEMY_DEFAULT_NAME;

            if (level >= BOSS_LEVEL_THRESHOLD)
            {
                _isBoss = true;
                if (Name == ENEMY_DEFAULT_NAME)
                    Name = BOSS_DEFAULT_NAME;
            }

            _enemyCount++;
        }

        /// <summary>
        /// Private constructor used to create a new enemy when loading enemy data from save.
        /// </summary>
        /// <param name="spawnX">X Coord of the enemy.</param>
        /// <param name="spawnY">Y Coord of the enemy.</param>
        /// <param name="name">Name of the enemy.</param>
        /// <param name="level">Level of the enemy.</param>
        /// <param name="health">Health of the enemy.</param>
        /// <param name="strength">Strength of the enemy.</param>
        /// <param name="defense">Defense of the enemy.</param>
        /// <param name="blockMult">Block multiplier of the enemy.</param>
        private Enemy(int spawnX, int spawnY, string name, int level, int health, int strength, int defense, double blockMult) : base(spawnX, spawnY, name, level, health, strength, defense, blockMult)
        {
            if (level >= BOSS_LEVEL_THRESHOLD)
                _isBoss = true;

            _enemyCount++;
        }

        /// <summary>
        /// Returns all the enemy data as a string, values divided by a divider character.
        /// </summary>
        /// <returns>A string containing all the enemy data.</returns>
        public string ExportSaveDataAsString()
        {
            char div = IMapObject.EXPORT_DIVIDER_CHAR;
            return $"{CurrentX}{div}{CurrentY}{div}{Name}{div}{Level}{div}{Health}{div}{Strength}{div}{Defense}{div}{BlockMultiplier}";
        }

        /// <summary>
        /// Creates a new enemy from the provided save data. Splits the string and tries to convert the values, then calls a constructor.
        /// </summary>
        /// <param name="saveDataString">The save data of the enemy.</param>
        /// <returns>An enemy object. If an exception occurs during conversion, returns null.</returns>
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

                return new Enemy(spawnX, spawnY, name, level, health, strength, defense, blockMult);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new enemy and adds it to the map, at the corresponding coordinates. Made to mitigate the problem that the enemy coordinates and actual location on the map can be different.
        /// </summary>
        /// <param name="map">The map to which the enemy is to be added.</param>
        /// <param name="spawnX">The starting x coordinate of the enemy.</param>
        /// <param name="spawnY">The starting y coordinate of the enemy.</param>
        /// <param name="level">The starting level of the enemy.</param>
        /// <param name="name">The name of the enemy. If no provided, uses default.</param>
        public static void NewEnemy(GameMap map, int spawnX, int spawnY, int level, string name = ENEMY_DEFAULT_NAME)
        {
            Enemy enemy = new Enemy(spawnX, spawnY,  level, name);

            map.AddThing(enemy, spawnX, spawnY);
        }

        /// <summary>
        /// Destructor. Substracts the enemy from the enemy count on deletion.
        /// </summary>
        ~Enemy()
        {
            _enemyCount--;
        }
    }
}
