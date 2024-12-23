using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class DefensePotion : Item
    {
        private const int DEFENSE_INCREASE = 5;
        private const string DISPLAY_NAME = "Defense Potion", DESCRIPTION = $"A defense potion that permanently increases your defense by 5.";

        public override string Name
        { get { return DISPLAY_NAME; } }
        public override string Description
        { get { return DESCRIPTION; } }

        public DefensePotion() : base()
        {

        }

        public override void Use(Player player)
        {
            player.Defense += DEFENSE_INCREASE;
        }
    }
}
