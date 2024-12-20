using PIIIProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        private Player _player;

        public Inventory(Player player)
        {
            InitializeComponent();
            _player = player;
            AllItems.ItemsSource = _player.Inventory;
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            Item tempItem = AllItems.SelectedItem as Item;
            if (tempItem is not null)
                ItemDescription.Text = tempItem.Description;
        }

        private void BtnUse_Clicked(object sender, RoutedEventArgs e)
        {
            Item tempItem = AllItems.SelectedItem as Item;
            if (tempItem is not null)
            {
                tempItem.Use(_player);
                _player.Inventory.Remove(tempItem);
                ItemDescription.Text = "";
            }
        }

        private void BtnBack_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
