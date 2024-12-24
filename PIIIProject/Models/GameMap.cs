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
        /// <summary>
        /// An enum of directions. Avoids magic numbers, improved code readability.
        /// </summary>
        public enum Direction
        { 
            Up,
            Down,
            Left,
            Right
        }

        // Constant
        public const char FLOOR_DISPLAY_CHAR = '.';

        // Backing field, is the map itself. The map is technically a 3-dimensional array, being a 2-dimensional array of lists.
        // It's like this because every cell (called tile in 2d games like this) is supposed to be able to contain multiple things.
        private List<IMapObject>[,] _logicMap;

        /// <summary>
        /// Readonly property for the logical map.
        /// </summary>
        public List<IMapObject>[,] LogicMap
        { 
            get 
            { 
                return _logicMap; 
            } 
        }

        /// <summary>
        /// Readonly, calculated property that returns a 2d array of characters. Is meant to display the map to the user/player.
        /// </summary>
        public char[,] DisplayMap
        {
            get
            {
                char[,] display = new char[_logicMap.GetLength(0), _logicMap.GetLength(1)];

                for (int y = 0; y < _logicMap.GetLength(0); y++)
                {
                    for (int x = 0; x < _logicMap.GetLength(1); x++)
                    {
                        // If a cell is empty, use the floor character. Otherwise, use the character of tha last item added to the list (logically the upermost item).
                        if (_logicMap[y, x].Count == 0)
                            display[y, x] = FLOOR_DISPLAY_CHAR;
                        else
                            display[y, x] = _logicMap[y, x][_logicMap[y, x].Count - 1].GetMapDisplayChar();
                    }
                }

                return display;
            }
        }

        /// <summary>
        /// Constructor for the map.
        /// </summary>
        /// <param name="height">Height/number of rows of the map.</param>
        /// <param name="width">Width/number of columns of the map.</param>
        public GameMap(int height, int width)
        {
            _logicMap = new List<IMapObject>[height, width];
            InitializeAllCells();
        }

        /// <summary>
        /// Initializes every cell of the map.
        /// </summary>
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
        
        /// <summary>
        /// Adds a thing to the map, if possible. If the coords are outside the map, does nothing.
        /// </summary>
        /// <param name="thing">The thing to add. If null, throws an exception.</param>
        /// <param name="thingX">The x coord of the thing.</param>
        /// <param name="thingY">The y coord of the thing.</param>
        /// <exception cref="ArgumentNullException">Exception thrown if the thing provided is null.</exception>
        public void AddThing(IMapObject thing, int thingX, int thingY)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;

            if (thing is null)
                throw new ArgumentNullException("The thing to add can't be null.");
            _logicMap[thingY, thingX].Add(thing);
        }

        /// <summary>
        /// Removes a thing from the map, if possible. If the coords are outside the map, does nothing.
        /// </summary>
        /// <param name="thing">The thing to remove.</param>
        /// <param name="thingX">The x coord of the thing.</param>
        /// <param name="thingY">The y coord of the thing.</param>
        /// <exception cref="ArgumentNullException">Exception thrown if the thing provided is null.</exception>
        /// <exception cref="Exception">Exception thrown if the item to remove is not found.</exception>
        public void RemoveThing(IMapObject thing, int thingX, int thingY)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;

            if (thing is null)
                throw new ArgumentNullException("The thing to remove can't be null.");

            if (!_logicMap[thingY, thingX].Contains(thing))
                throw new Exception("There's no such thing there.");

            _logicMap[thingY, thingX].Remove(thing);
        }

        /// <summary>
        /// Moves a thing one tile in a given direction. Adds the item to the next cell and removes it from the previous.
        /// </summary>
        /// <param name="thing">Thing to move.</param>
        /// <param name="thingX">The x coord of the thing.</param>
        /// <param name="thingY">The y coord of the thing.</param>
        /// <param name="direction">The direction in which to move the thing.</param>
        /// <exception cref="ArgumentNullException">Exception thrown if the thing provided is null.</exception>
        /// <exception cref="Exception">Exception thrown if the item to move is not found.</exception>
        public void MoveThing(IMapObject thing, int thingX, int thingY, Direction direction)
        {
            if (thingY < 0 || thingY >= LogicMap.GetLength(0) || thingX < 0 || thingX >= LogicMap.GetLength(1))
                return;

            if (thing is null)
                throw new ArgumentNullException("The thing to move can't be null.");

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
                    AddThing(thing, thingX, thingY);
                    break;
            }

            _logicMap[thingY, thingX].Remove(thing);
        }

        /// <summary>
        /// Draws a wall on the map. Can draw diagonal walls thanks to math.
        /// </summary>
        /// <param name="startX">The x coord of the starting point.</param>
        /// <param name="startY">The y coord of the starting point.</param>
        /// <param name="endX">The x coord of the ending point.</param>
        /// <param name="endY">The y coord of the ending point.</param>
        public void AddWall(int startX, int startY, int endX, int endY)
        {
            int temp;
            decimal coord;
            decimal increment = 0;

            // swaps the start and end coords if the end coords are smaller that the start coords
            if (startX >= endX && startY >= endY)
            {
                temp = startX;
                startX = endX;
                endX = temp;

                temp = startY;
                startY = endY;
                endY = temp;
            }

            // If the wall is more horizontal
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
            // If the wall is more vertical
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

        /// <summary>
        /// Checks it a map cell contains any collidable objects. If the coords are outside the map, returns false.
        /// </summary>
        /// <param name="cellX">The cell x coord of the cell to check.</param>
        /// <param name="cellY">The cell y coord of the cell to check.</param>
        /// <returns>True if it has, false otherwise.</returns>
        private bool MapCellHasCollidable(int cellX, int cellY)
        {
            if (cellY < 0 || cellY >= LogicMap.GetLength(0) || cellX < 0 || cellX >= LogicMap.GetLength(1))
                return false;

            foreach (IMapObject thing in LogicMap[cellY, cellX])
            {
                if (thing is ICollidable)
                    return true;
            }

            return false;
        }
    }
}
