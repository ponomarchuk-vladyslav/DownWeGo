using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace PIIIProject.Models
{
    public static class SaverLoader
    {
        private class SaveData
        {
            public struct Thing
            {
                public IMapObject ThingData;
                public int ThingX, ThingY;
            }
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
                            ThingData = thing,
                            ThingX = col,
                            ThingY = row
                        });
                    }
                }
            }

            string saveDataJson = JsonConvert.SerializeObject(saveData,
  new JsonSerializerSettings()
  {
      TypeNameHandling = TypeNameHandling.Auto
  });

            File.WriteAllText(path, saveDataJson);
        }
        public static void Load(ref Player player, ref GameMap map, string path = AUTOSAVE_PATH)
        {
            string saveDataJson = File.ReadAllText(path);

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(saveDataJson,
                new JsonSerializerSettings()
  {
                TypeNameHandling = TypeNameHandling.Auto
  });

            if (saveData is null)
                throw new Exception("Save is null.");

            map = new GameMap(saveData.Rows, saveData.Columns);

            for (int i = saveData.Things.Count - 1; i >= 0; i--)
            {
                map.AddThing(saveData.Things[i].ThingData, saveData.Things[i].ThingX, saveData.Things[i].ThingY);
                if (map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX][map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX].Count - 1] is Player)
                {
                    player = map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX][map.LogicMap[saveData.Things[i].ThingY, saveData.Things[i].ThingX].Count - 1] as Player;
                }
            }
        }
    }
}
