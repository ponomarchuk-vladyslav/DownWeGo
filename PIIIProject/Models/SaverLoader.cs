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
        private class SaveData
        {
            public struct Thing
            {
                public Type ThingType;
                public int ThingX, ThingY;
                public string ThingData;
            }

            public ObservableCollection<Item> Inventory;
            public List<Thing> Things = new List<Thing>();
            public int Rows, Columns;
        }

        public const string AUTOSAVE_PATH = "./autosave.save";

        public static void Save(Player player, GameMap map, string path = AUTOSAVE_PATH)
        {
            SaveData saveData = new SaveData()
            {
                Rows = map.LogicMap.GetLength(0),
                Columns = map.LogicMap.GetLength(1)
            };

            for (int row = 0; row < map.LogicMap.GetLength(0); row++)
            {
                for (int col = 0; col < map.LogicMap.GetLength(1); col++)
                {
                    foreach (IMapObject thing in map.LogicMap[row, col])
                    {
                        saveData.Things.Add(new SaveData.Thing
                        {
                            ThingType = thing.GetType(),
                            ThingData = thing.ExportSaveDataAsString(),
                            ThingX = col,
                            ThingY = row
                        });

                        if (thing is Player)
                        {
                            saveData.Inventory = (thing as Player).Inventory;
                        }
                    }
                }
            }

            string saveDataJson = JsonConvert.SerializeObject(saveData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            File.WriteAllText(path, saveDataJson);
        }
        public static void Load(ref Player player, ref GameMap map, string path = AUTOSAVE_PATH)
        {
            string saveDataJson = File.ReadAllText(path);

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(saveDataJson, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            if (saveData is null)
                throw new Exception("Save is null.");

            map = new GameMap(saveData.Rows, saveData.Columns);

            for (int i = saveData.Things.Count - 1; i >= 0; i--)
            {
                IMapObject thing;
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
                    throw new Exception(saveData.Things[i].ThingType.ToString());

                map.AddThing(thing, saveData.Things[i].ThingX, saveData.Things[i].ThingY);

                if (thing is Player)
                {
                    player = map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX][map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX].Count - 1] as Player;
                    player.Inventory = saveData.Inventory;
                }
            }
        }
    }
}
