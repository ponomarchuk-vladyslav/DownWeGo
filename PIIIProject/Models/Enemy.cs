using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Enemy : Creature, IMapObject
    {
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

        public Enemy() : base()
        {

        }

        public Enemy(GameMap map, int spawnX, int spawnY, int level, string name = ENEMY_DEFAULT_NAME) : base(spawnX, spawnY, name, level * START_HEALTH, level * START_STRENGTH, level * START_DEFENSE)
        {
            if (level >= BOSS_LEVEL_THRESHOLD)
            {
                _isBoss = true;
                if (Name == ENEMY_DEFAULT_NAME)
                    Name = BOSS_DEFAULT_NAME;
            }
            map.AddThing(this, spawnX, spawnY);
        }
    }
}
