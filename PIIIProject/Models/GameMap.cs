using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PIIIProject.Models
{
    public class GameMap
    {
        public enum Direction
        { 
            Up,
            Down,
            Left,
            Right
        }

        public const char DEFAULT_DISPLAY_CHAR = '.';

        private List<IMapObject>[,] _logicMap;

        public List<IMapObject>[,] LogicMap
        { get { return _logicMap; } }

        public char[,] DisplayMap
        {
            get
            {
                char[,] display = new char[_logicMap.GetLength(0), _logicMap.GetLength(1)];

                for (int y = 0; y < _logicMap.GetLength(0); y++)
                {
                    for (int x = 0; x < _logicMap.GetLength(1); x++)
                    {
                        if (_logicMap[_logicMap.GetLength(0) - (y + 1), x].Count == 0)
                            display[y, x] = DEFAULT_DISPLAY_CHAR;
                        else
                            display[y, x] = _logicMap[_logicMap.GetLength(0) - (y + 1), x][_logicMap[_logicMap.GetLength(0) - (y + 1), x].Count - 1].GetMapDisplayChar();
                    }
                }

                return display;
            }
        }

        public GameMap(int height, int width)
        {
            _logicMap = new List<IMapObject>[height, width];
            InitializeAllCells();
        }

        private void InitializeAllCells()
        {
            for (int y = 0; y < _logicMap.GetLength(0); y++)
            {
                for (int x = 0; x < _logicMap.GetLength(1); x++)
                {
                    _logicMap[y, x] = new List<IMapObject>();
                }
            }
        }

        public void AddThing(IMapObject thing, int thingX, int thingY)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;

            if (thing is null)
                throw new Exception("Thing can't be null.");
            _logicMap[thingY, thingX].Add(thing);
        }

        public void RemoveThing(IMapObject thing, int thingX, int thingY)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;

            if (!_logicMap[thingY, thingX].Contains(thing))
                throw new Exception("There's no such thing there.");
            _logicMap[thingY, thingX].Remove(thing);
        }

        public void MoveThing(IMapObject thing, int thingX, int thingY, Direction direction)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;
                
            if (!_logicMap[thingY, thingX].Contains(thing))
                throw new Exception("The thing you want to move is not there.");

            switch (direction)
            {
                case Direction.Up:
                    AddThing(thing, thingX, thingY + 1);
                    break;
                case Direction.Down:
                    AddThing(thing, thingX, thingY - 1);
                    break;
                case Direction.Left:
                    AddThing(thing, thingX - 1, thingY);
                    break;
                case Direction.Right:
                    AddThing(thing, thingX + 1, thingY);
                    break;
                default:
                    break;
            }

            _logicMap[thingY, thingX].Remove(thing);
        }

        public void AddWall(int startX, int startY, int endX, int endY)
        {
            int temp;
            decimal coord;
            decimal increment = 0;

            if ((startX > endX || startX == endX) && (startY > endY || startY == endY))
            {
                temp = startX;
                startX = endX;
                endX = temp;

                temp = startY;
                startY = endY;
                endY = temp;
            }

            if (endX - startX > endY - startY)
            {
                increment = (decimal)(endY - startY) / (decimal)(endX - startX);

                coord = startY;
                for (int x = startX; x <= endX; x++)
                {
                    if ((x - startX) % (endX - startX) == 0)
                    {
                        coord = Math.Round(coord);
                    }
                    if (!MapCellHasCollidable(x, (int)Math.Ceiling(coord)))
                    {
                        AddThing(new Wall(), x, (int)Math.Ceiling(coord));
                    }
                    if (!MapCellHasCollidable(x, (int)coord))
                    {
                        AddThing(new Wall(), x, (int)coord);
                    }
                    coord += increment;
                }
            }
            else
            {
                increment = (decimal)(endX - startX) / (decimal)(endY - startY);

                coord = startX;
                for (int y = startY; y <= endY; y++)
                {
                    if ((y - startY) % (endY - startY) == 0)
                    {
                        coord = Math.Round(coord);
                    }
                    if (!MapCellHasCollidable((int)Math.Ceiling(coord), y))
                    {
                        AddThing(new Wall(), (int)Math.Ceiling(coord), y);
                    }
                    if (!MapCellHasCollidable((int)coord, y))
                    {
                        AddThing(new Wall(), (int)coord, y);
                    }
                    coord += increment;
                }
            }
        }

        public bool MapCellHasCollidable(int cellX, int cellY)
        {
            foreach (IMapObject thing in LogicMap[cellY, cellX])
            {
                if (thing is ICollidable)
                    return true;
            }

            return false;
        }
    }
}
