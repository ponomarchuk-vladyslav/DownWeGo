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
            if (_player.Inventory.Count > 0)
                ToggleTxtb_NoItemsVisibility();
            
            PlayerStats.Text = _player.AllStats;
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
                PlayerStats.Text = _player.AllStats;
                if (_player.Inventory.Count == 0)
                    ToggleTxtb_NoItemsVisibility();
            }
        }

        private void BtnBack_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ToggleTxtb_NoItemsVisibility()
        {
            if (Txtb_NoItems.Visibility == Visibility.Visible)
                Txtb_NoItems.Visibility = Visibility.Hidden;
            else if (Txtb_NoItems.Visibility == Visibility.Hidden) //Also could try making it if(Visibility != Visibility.Visible)
                Txtb_NoItems.Visibility = Visibility.Visible;
            

            
        }
    }
}
