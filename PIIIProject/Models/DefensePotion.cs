using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class DefensePotion : Item, IMapObject
    {
        // Constants
        private const int DEFENSE_INCREASE = 5;
        private const string DISPLAY_NAME = "Defense Potion", DESCRIPTION = $"A defense potion that permanently increases your defense by 5.";

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
        public DefensePotion() : base()
        {

        }

        /// <summary>
        /// Adds defense to the player.
        /// </summary>
        /// <param name="player">The player who should have his defense increased.</param>
        public override void Use(Player player)
        {
            player.AddDefense(DEFENSE_INCREASE);
        }

        /// <summary>
        /// Creates a new defense potion from the provided save data and returns the potion. The defense potion doesn't have any data to store, so if any data is provided, this means that the wrong data is provided.
        /// </summary>
        /// <param name="saveDataString">The save data as a string, created by the export method.</param>
        /// <returns>A new defense potion created from the provided data.</returns>
        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            if (string.IsNullOrEmpty(saveDataString))
                return new DefensePotion();
            return null;
        }
    }
}
