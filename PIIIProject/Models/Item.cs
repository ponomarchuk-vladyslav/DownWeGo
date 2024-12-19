using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public abstract class Item : IMapObject
    {
        public const char ITEM_DISPLAY_CHAR = '$';

        public Item() 
        {
            
        }

        public abstract void Use(Player player);
        
        public char GetMapDisplayChar()
        { 
            return ITEM_DISPLAY_CHAR;
        }
    }
}
