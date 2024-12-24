using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Enemy : Creature, IMapObject, ICollidable
    {
        private static int _enemyCount = 0;

        public static int EnemyCount
        {
            get { return _enemyCount; }
        }

        public const string ENEMY_DEFAULT_NAME = "Skeleton";
        public const string BOSS_DEFAULT_NAME = "Abomination";
        public const char ENEMY_DISPLAY_CHAR = 'x';
        public const char BOSS_DISPLAY_CHAR = 'X';
        public const int BOSS_LEVEL_THRESHOLD = 15;
        public const int START_HEALTH = 10, START_STRENGTH = 2, START_DEFENSE = 1;

        private bool _isBoss;

        public char GetMapDisplayChar()
        {
            if (_isBoss)
                return BOSS_DISPLAY_CHAR;
            return ENEMY_DISPLAY_CHAR;  
        }

        public Enemy(int spawnX, int spawnY, int level, string name = ENEMY_DEFAULT_NAME) : base(spawnX, spawnY, name, level, START_HEALTH, START_STRENGTH, START_DEFENSE)
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

        private Enemy(int spawnX, int spawnY, string name, int level, int health, int strength, int defense, double blockMult) : base(spawnX, spawnY, name, level, health, strength, defense, blockMult)
        {
            if (level >= BOSS_LEVEL_THRESHOLD)
                _isBoss = true;

            _enemyCount++;
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

                return new Enemy(spawnX, spawnY, name, level, health, strength, defense, blockMult);
            }
            catch
            {
                return null;
            }
        }

        ~Enemy()
        {
            _enemyCount--;
        }
    }
}
