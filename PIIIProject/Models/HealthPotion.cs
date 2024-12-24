using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class HealthPotion : Item, IMapObject
    {
        private const int HEALTH_RESTORED = 5;
        private const string DISPLAY_NAME = "Health Potion", DESCRIPTION = $"A health potion that restores 5 health.";

        public override string Name
        { get { return DISPLAY_NAME; } }
        public override string Description
        { get { return DESCRIPTION; } }

        public HealthPotion() : base()
        {

        }

        public override void Use(Player player)
        {
            player.Heal(HEALTH_RESTORED);
        }

        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            if (string.IsNullOrEmpty(saveDataString))
                return new HealthPotion();
            return null;
        }
    }
}
