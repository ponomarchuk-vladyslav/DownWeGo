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
        public const char ENEMY_DISPLAY_CHAR = 'X';
        public const int START_HEALTH = 10, START_STRENGTH = 2, START_DEFENSE = 1;
        public char GetMapDisplayChar()
        { 
            return ENEMY_DISPLAY_CHAR;  
        }

        public Enemy() : base()
        {

        }

        public Enemy(GameMap map, int spawnX, int spawnY, int level, string name = ENEMY_DEFAULT_NAME) : base(spawnX, spawnY, name, level * START_HEALTH, level * START_STRENGTH, level * START_DEFENSE)
        {
            map.AddThing(this, spawnX, spawnY);
        }
    }
}
