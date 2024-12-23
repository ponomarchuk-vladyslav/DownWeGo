using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    class Escape : IMapObject
    {
        public const char ESCAPE_DISPLAY_CHAR = 'M';

        public Escape()
        {

        }

        public char GetMapDisplayChar()
        {
            return ESCAPE_DISPLAY_CHAR;
        }
    }
}
