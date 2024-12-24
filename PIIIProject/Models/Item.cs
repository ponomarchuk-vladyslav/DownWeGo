using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    // Unfortunately, I couldn't apply the IMapObject here, I have to apply it to every item individually because of the static Load method, which is supposed to return a new object.
    public abstract class Item
    {
        // Constants
        public const char ITEM_DISPLAY_CHAR = '$';

        /// <summary>
        /// The default constructor. I keep it here just in case I need it.
        /// </summary>
        public Item() 
        {
            
        }

        /// <summary>
        /// Read-only property for accessing the name of the item.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Read-only property for accessing the description of the item.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// A method to use the item. To be implemented.
        /// </summary>
        /// <param name="player">The player who should be affected.</param>
        public abstract void Use(Player player);
        
        /// <summary>
        /// Gets the character that represents the item.
        /// </summary>
        /// <returns>A character</returns>
        public char GetMapDisplayChar()
        { 
            return ITEM_DISPLAY_CHAR;
        }

        /// <summary>
        /// Exports the item data. As most items have no data to store, returns an empty string by default.
        /// </summary>
        /// <returns>A string with all the data of the item.</returns>
        public virtual string ExportSaveDataAsString()
        {
            return "";
        }
    }
}
