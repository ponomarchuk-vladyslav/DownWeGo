using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject
{
    /// <summary>
    /// Interface that designates an object as a map object. Allows to easily process different objects together.
    /// </summary>
    public interface IMapObject
    {
        const char EXPORT_DIVIDER_CHAR = ':';
        /// <summary>
        /// Gets the character that represents the object. The character is to be determined by the class.
        /// </summary>
        /// <returns>A character.</returns>
        char GetMapDisplayChar();

        /// <summary>
        /// Exports all the object data as a string, divided by a divider character. 
        /// </summary>
        /// <returns>A string containing all the data of a string.</returns>
        string ExportSaveDataAsString();

        /// /// <summary>
        /// Creates a new object from the provided save data and returns the object.
        /// </summary>
        /// <param name="saveDataString">The save data as a string, created by the export method.</param>
        /// <returns>A new object created from the provided data.</returns>
        static IMapObject LoadSaveDataFromString(string saveDataString)
        {
            return null;
        }
    }
}
