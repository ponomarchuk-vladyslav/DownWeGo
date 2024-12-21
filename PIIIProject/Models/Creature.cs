using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public abstract class Creature
    {
        private string _name;
        private int _strength, _defense;
        private double _health, _blockMultiplier;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("The creature must have a name.");
                _name = value;
            }
        }
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
        private int Defense
        {
            get { return _defense; }
            set
            {
                _defense = value;
                if (value < 0)
                    _defense = 0;
            }
        }
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
        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public string AllStats
        {
            get
            {
                return $"{Name} stats:\n" +
                    $"Health: {Health}\n" +
                    $"Strength : {Strength}\n" +
                    $"Defense: {Defense}";
            }
        }

        public Creature(string name, int health, int strength, int defense)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Defense = defense;
        }

        public void Hurt(int amount)
        {
            Health -= (amount - (Defense * BlockMultiplier));
        }
        public void Heal(int amount)
        {
            Health += amount;
        }
    }
}
