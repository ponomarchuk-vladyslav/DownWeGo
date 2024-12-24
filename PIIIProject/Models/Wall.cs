using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Wall : IMapObject, ICollidable
    {
        // Constant
        public const char WALL_DISPLAY_CHAR = '#';

        /// <summary>
        /// Default constructor. I kept it just in case I'll need it.
        /// </summary>
        public Wall()
        {

        }

        /// <summary>
        /// Gets the character representing a wall.
        /// </summary>
        /// <returns>A character.</returns>
        public char GetMapDisplayChar()
        { 
            return WALL_DISPLAY_CHAR; 
        }

        /// <summary>
        /// Exports the data of this object as a string. As a wall has no data, exports an empty string.
        /// </summary>
        /// <returns>An empty string.</returns>
        public string ExportSaveDataAsString()
        {
            return "";
        }

        /// <summary>
        /// Creates a new wall and returns it. If the data string is not empty, that means that this is a string for another object, thus it returns null.
        /// </summary>
        /// <param name="saveDataString">The data. Should be empty.</param>
        /// <returns>A new wall if the data string is empty, null otherwise.</returns>
        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            if (string.IsNullOrEmpty(saveDataString))
                return new Wall();
            return null;
        }
    }
}
