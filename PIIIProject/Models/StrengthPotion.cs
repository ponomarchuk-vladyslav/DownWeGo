using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class StrengthPotion : Item
    {
        private const int STRENGTH_INCREASE = 5;
        private const string DISPLAY_NAME = "Strength Potion", DESCRIPTION = $"A strength potion that permanently increases your strength by 5.";

        public override string Name
        { get { return DISPLAY_NAME; } }
        public override string Description
        { get { return DESCRIPTION; } }

        public StrengthPotion() : base()
        {

        }

        public override void Use(Player player)
        {
            player.Strength += STRENGTH_INCREASE;
        }
    }
}
