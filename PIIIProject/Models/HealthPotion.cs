using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class HealthPotion : Item
    {
        private const int HEALTH_RESTORED = 5;
        private const string DISPLAY_NAME = "Health Potion", DESCRIPTION = $"A health potion that restores 5 health.";

        public HealthPotion() : base()
        {

        }

        public override void Use(Player player)
        {
            player.Health += HEALTH_RESTORED;
        }
    }
}
