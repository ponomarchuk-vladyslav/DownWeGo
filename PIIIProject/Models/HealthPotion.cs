using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class HealthPotion : Item, IMapObject
    {
        // Constants
        private const int HEALTH_RESTORED = 5;
        private const string DISPLAY_NAME = "Health Potion", DESCRIPTION = $"A health potion that restores 5 health.";

        /// <summary>
        /// Read-only property for accessing the name of the item.
        /// </summary>
        public override string Name
        { 
            get 
            { 
                return DISPLAY_NAME; 
            } 
        }
        /// <summary>
        /// Read-only property for accessing the description of the item.
        /// </summary>
        public override string Description
        { 
            get 
            {
                return DESCRIPTION;
            }
        }

        /// <summary>
        /// The default constructor. I keep it here to call the base constructor, however it doesn't seem to be required, c# does it by itself.
        /// </summary>
        public HealthPotion() : base()
        {

        }

        /// <summary>
        /// Adds health to the player.
        /// </summary>
        /// <param name="player">The player who should have his health increased.</param>
        public override void Use(Player player)
        {
            player.Heal(HEALTH_RESTORED);
        }

        /// <summary>
        /// Creates a new health potion from the provided save data and returns the potion. The health potion doesn't have any data to store, so if any data is provided, this means that the wrong data is provided.
        /// </summary>
        /// <param name="saveDataString">The save data as a string, created by the export method.</param>
        /// <returns>A new health potion created from the provided data.</returns>
        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            if (string.IsNullOrEmpty(saveDataString))
                return new HealthPotion();
            return null;
        }
    }
}
