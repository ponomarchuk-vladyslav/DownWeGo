using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Item : IMapObject
    {
        public const char ITEM_DISPLAY_CHAR = '$';

        public Item() 
        {
            
        }
        
        public char GetMapDisplayChar()
        { return ITEM_DISPLAY_CHAR; }
    }
}
