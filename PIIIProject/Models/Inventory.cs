using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    static class InventoryList
    {
        private static ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public static ObservableCollection<Item> RemoveItem(int index)
        {
            _items.RemoveAt(index);
            return _items;
        }
        public static ObservableCollection<Item> AddItem(Item item)
        {
            if(item != null)
                _items.Add(item);
            return _items;
        }
        public static ObservableCollection<Item> GetCollection()
    {
            return _items;
        }


    }
}
