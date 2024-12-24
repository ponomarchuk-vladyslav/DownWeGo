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
        // Private fields
        private Player _player;
        private GameMap _map;
        private Enemy _enemy;

        /// <summary>
        /// Constructor for a new Inventory window. If a non-null enemy is provided, considers that the inventory window was called by a combat window. Otherwise, considers that it was  called by the map.
        /// </summary>
        /// <param name="map">The map is not really used by the inventory, it's simply passed to the new window created when the inventory is closed.</param>
        /// <param name="player">The player to whom the inventory belongs.</param>
        /// <param name="enemy">The enemy. Again, not used, serves to determine if the inventory window was called by the map or combat. Passed to a new combat window.</param>
        public Inventory(GameMap map, Player player, Enemy enemy = null)
        {
            InitializeComponent();

            _player = player;
            _map = map;
            _enemy = enemy;

            AllItems.ItemsSource = _player.Inventory;
            if (_player.Inventory.Count > 0)
                ToggleTxtb_NoItemsVisibility();
            
            PlayerStats.Text = _player.AllStats;
        }

        /// <summary>
        /// Handles when an inventory item is clicked. Updates the description.
        /// </summary>
        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            Item tempItem = AllItems.SelectedItem as Item;
            if (tempItem is not null)
                ItemDescription.Text = tempItem.Description;
        }

        /// <summary>
        /// Handles the use button click. Uses the item on the player, removes it, resets the description because no item is selected and updates the player stats display.
        /// </summary>
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

        /// <summary>
        /// Handles the back to game button click. Opens a new window and closes this one. The new window is determined by the previos window, which is indicated by the enemy variable.
        /// </summary>
        private void BtnBack_Clicked(object sender, RoutedEventArgs e)
        {
            if (_enemy is null)
            {
                Game game = new Game(_player, _map);
                game.Show();
            }
            else
            {
                Combat combat = new Combat(_map, _player, _enemy);
                combat.Show();
            }

            this.Close();
        }

        /// <summary>
        /// Toggles the the visibility of the text that says there's no items.
        /// </summary>
        private void ToggleTxtb_NoItemsVisibility()
        {
            if (Txtb_NoItems.Visibility == Visibility.Visible)
                Txtb_NoItems.Visibility = Visibility.Hidden;
            else if (Txtb_NoItems.Visibility == Visibility.Hidden)
                Txtb_NoItems.Visibility = Visibility.Visible;
        }
    }
}
