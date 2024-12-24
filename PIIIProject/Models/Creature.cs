using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public abstract class Creature
    {
        // Backing fields
        private string _name;
        private int _level, _strength, _defense;
        private double _health, _blockMultiplier;
        protected int _currentX, _currentY;

        /// <summary>
        /// The X Coordinate of the creature on the map. Used to track where it is so it can be removed.
        /// </summary>
        public int CurrentX
        {
            get { return _currentX; }
            protected set
            {
                _currentX = value;
                if (value < 0)
                    _currentX = 0;
            }
        }
        /// <summary>
        /// The Y Coordinate of the creature on the map. Used to track where it is so it can be removed.
        /// </summary>
        public int CurrentY
        {
            get { return _currentY; }
            protected set
            {
                _currentY = value;
                if (value < 0)
                    _currentY = 0;
            }
        }

        /// <summary>
        /// The Name of the creature.
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("The creature must have a name.");
                _name = value;
            }
        }
        /// <summary>
        /// The level of the creature.
        /// </summary>
        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                if (value < 1)
                    _level = 1;
            }
        }
        /// <summary>
        /// The health of the creature
        /// </summary>
        public double Health
        {
            get { return _health; }
            private set
            {
                _health = value;
                if (value < 0)
                    _health = 0;
            }
        }
        /// <summary>
        /// The strength of the creature. Determines the damage the creature deals.
        /// </summary>
        public int Strength
        {
            get { return _strength; }
            private set
            {
                _strength = value;
                if (value < 0)
                    _strength = 0;
            }
        }
        /// <summary>
        /// The defense of the creature. Provides a flat reduction on damage taken.
        /// </summary>
        public int Defense
        {
            get { return _defense; }
            private set
            {
                _defense = value;
                if (value < 0)
                    _defense = 0;
            }
        }
        /// <summary>
        /// The block multiplier of the creature. Multiplies the defense of the creature. Can be used to buff/enhance defense or debuff/reduce it temporarily.
        /// </summary>
        public double BlockMultiplier
        {
            get { return _blockMultiplier; }
            set
            {
                _blockMultiplier = value;
                if (value < 0)
                    _blockMultiplier = 0;
            }
        }
        /// <summary>
        /// Checks if the enemy is dead. Dead is defined as being at 0 health.
        /// </summary>
        public bool IsDead
        {
            get { return Health <= 0; }
        }

        /// <summary>
        /// Returns the formated string with all the stats of the creaturs.
        /// </summary>
        public string AllStats
        {
            get
            {
                return $"Lv. {Level} {Name} stats:\n" +
                    $"Health: {Health}\n" +
                    $"Strength : {Strength}\n" +
                    $"Defense: {Defense}";
            }
        }

        /// <summary>
        /// Constructor used when creating a new creature
        /// </summary>
        /// <param name="spawnX">The starting x coordinate of the creature.</param>
        /// <param name="spawnY">The starting y coordinate of the creature.</param>
        /// <param name="name">The name of the creature.</param>
        /// <param name="level">The level of the creature.</param>
        /// <param name="healthPerLevel">Health gained per level. Starting health will be this multiplied by the level.</param>
        /// <param name="strengthPerLevel">Strength gained per level. Starting strength will be this multiplied by the level.</param>
        /// <param name="defensePerLevel">Defense gained per level. Starting defense will be this multiplied by the level.</param>
        public Creature(int spawnX, int spawnY, string name, int level, int healthPerLevel, int strengthPerLevel, int defensePerLevel)
        {
            CurrentX = spawnX;
            CurrentY = spawnY;

            Name = name;

            Level = level;

            Health = healthPerLevel * Level;
            Strength = strengthPerLevel * Level;
            Defense = defensePerLevel * Level;
        }
        /// <summary>
        /// Constructor that sets all fields to the provided values. To be used when loading object from string.
        /// </summary>
        /// <param name="spawnX">The starting x coordinate of the creature.</param>
        /// <param name="spawnY">The starting y coordinate of the creature.</param>
        /// <param name="name">The name of the creature.</param>
        /// <param name="level">The level of the creature.</param>
        /// <param name="strength">The strength of the creature.</param>
        /// <param name="defense">The defense of the creature.</param>
        /// <param name="blockMult">The block multiplier of the creature.</param>
        public Creature(int spawnX, int spawnY, string name, int level, int health, int strength, int defense, double blockMult)
        {
            CurrentX = spawnX;
            CurrentY = spawnY;

            Name = name;

            Level = level;
            Health = health;
            Strength = strength;
            Defense = defense;
            BlockMultiplier = blockMult;
        }

        /// <summary>
        /// Hurts (decreases health from the creature) by the given value. Amount is reduced by defense. If the amount is negative, does nothing.
        /// </summary>
        /// <param name="amount">Amount of health substracted.</param>
        public void Hurt(int amount)
        {
            double damage = amount - (Defense * BlockMultiplier);
            if (damage > 0)
                Health -= damage;
        }
        /// <summary>
        /// Heals (increases health) the creature by the given value. Negative amount does nothing.
        /// </summary>
        /// <param name="amount">Amount of health added.</param>
        public void Heal(int amount)
        {
            if (amount > 0)
                Health += amount;
        }

        /// <summary>
        /// Increases the creature's strength by the given amount. If 0 or negative, does nothing.
        /// </summary>
        /// <param name="amount">The amount by which to increase.</param>
        public void AddStrength(int amount)
        {
            if (amount > 0)
                Strength += amount;
        }
        /// <summary>
        /// Increases the creature's defense by the given amount. If 0 or negative, does nothing.
        /// </summary>
        /// <param name="amount">The amount by which to increase.</param>
        public void AddDefense(int amount)
        {
            if (amount > 0)
                Defense += amount;
        }
    }
}
