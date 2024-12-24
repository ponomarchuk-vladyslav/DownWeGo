using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Wall : IMapObject, ICollidable
    {
        public const char WALL_DISPLAY_CHAR = '#';

        public Wall()
        {

        }

        public char GetMapDisplayChar()
        { 
            return WALL_DISPLAY_CHAR; 
        }

        public string ExportSaveDataAsString()
        {
            return "";
        }

        public static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            if (string.IsNullOrEmpty(saveDataString))
                return new Wall();
            return null;
        }
    }
}
