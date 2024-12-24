using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace PIIIProject.Models
{
    public static class SaverLoader
    {
        /// <summary>
        /// A class used to store the save data.
        /// </summary>
        private class SaveData
        {
            /// <summary>
            /// Struct storing the save data of an object as a string, it's coordinates and it's type.
            /// </summary>
            public struct Thing
            {
                public Type ThingType;
                public int ThingX, ThingY;
                public string ThingData;
            }

            // The array of structs for storing objects, an array to store the player inventory and the size of the map.
            public ObservableCollection<Item> Inventory;
            public List<Thing> Things = new List<Thing>();
            public int Rows, Columns;
        }

        // Default path for the save file. Was used for testing.
        public const string AUTOSAVE_PATH = "./autosave.save";

        /// <summary>
        /// Saves the player and map data of the game into a file formated in JSON.
        /// </summary>
        /// <param name="player">The player to save</param>
        /// <param name="map">The map to save</param>
        /// <param name="path">The location where the file should be saved.</param>
        public static void Save(Player player, GameMap map, string path = AUTOSAVE_PATH)
        {
            // Creates a new savedata object and stores the dimensions of the map
            SaveData saveData = new SaveData()
            {
                Rows = map.LogicMap.GetLength(0),
                Columns = map.LogicMap.GetLength(1)
            };

            // Cycles through all the cells of the map and gets every item it finds
            for (int row = 0; row < map.LogicMap.GetLength(0); row++)
            {
                for (int col = 0; col < map.LogicMap.GetLength(1); col++)
                {
                    foreach (IMapObject thing in map.LogicMap[row, col])
                    {
                        // Adds the thing type, data and coords to the array in the savedata object
                        saveData.Things.Add(new SaveData.Thing
                        {
                            ThingType = thing.GetType(),
                            ThingData = thing.ExportSaveDataAsString(),
                            ThingX = col,
                            ThingY = row
                        });

                        // If the thing is the player, stores the inventory.
                        if (thing is Player)
                        {
                            saveData.Inventory = (thing as Player).Inventory;
                        }
                    }
                }
            }

            // Converts/Serializes the savedata object to a JSON string
            string saveDataJson = JsonConvert.SerializeObject(saveData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            // Writes the JSON string to the file
            File.WriteAllText(path, saveDataJson);
        }
        /// <summary>
        /// Loads all the data from a file and recreates the game state from it.
        /// </summary>
        /// <param name="player">The player pointer that should point to the player. Supposed to be null at the beginning.</param>
        /// <param name="map">The map pointer. Supposed to be null at the beginning.</param>
        /// <param name="path">The path of the file to load the data from.</param>
        /// <exception cref="Exception">Exception thrown if something goes wrong.</exception>
        public static void Load(ref Player player, ref GameMap map, string path = AUTOSAVE_PATH)
        {
            // Reads all the data from the file
            string saveDataJson = File.ReadAllText(path);

            // Tries to convert the data to a SaveData object.
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(saveDataJson, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            if (saveData is null)
                throw new Exception("SaveData is null. The save may be corrupted, emty, etc.");

            // Creates a new map with the dimensions stored in the save.
            map = new GameMap(saveData.Rows, saveData.Columns);

            // For each object stored, tries to recognize it's type, call the associated loading method and add the resulting object to the map.
            for (int i = saveData.Things.Count - 1; i >= 0; i--)
            {
                IMapObject thing;

                // I know it's ugly, I couldn't find how to do it better.
                if (saveData.Things[i].ThingType == typeof(Player))
                    thing = Player.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(Enemy))
                    thing = Enemy.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(Wall))
                    thing = Wall.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(Escape))
                    thing = Escape.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(HealthPotion))
                    thing = HealthPotion.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(StrengthPotion))
                    thing = StrengthPotion.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else if (saveData.Things[i].ThingType == typeof(DefensePotion))
                    thing = DefensePotion.LoadSaveDataFromString(saveData.Things[i].ThingData);
                else
                    throw new Exception($"The type {saveData.Things[i].ThingType.ToString()} has not been recognized. If you are using it, please add it to the saverloader class if else block.");

                if (thing is null)
                    throw new Exception("The loaded object is null.");

                map.AddThing(thing, saveData.Things[i].ThingX, saveData.Things[i].ThingY);

                // If the thing is a player, point the pointer to it and copy the savedata inventory to it.
                if (thing is Player)
                {
                    player = map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX][map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX].Count - 1] as Player;
                    player.Inventory = saveData.Inventory;
                }
            }
        }
    }
}
