using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
using PIIIProject.Models;

namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for Combat.xaml
    /// </summary>
    public partial class Combat : Window
    {
        // Default block multiplier. That means if somebody block, their defense will multiply by this amount for 1 turn.
        const double BLOCK_MULTIPLIER = 1.5;
        const int TURN_DELAY = 250;

        // Internal fields
        private GameMap _map;
        private Player _player;
        private Enemy _enemy;

        /// <summary>
        /// Constructor for the window. Sets the fields to the corresponding values and updates the display.
        /// </summary>
        /// <param name="map">The map on which the combat is happening.</param>
        /// <param name="player">The player that initiated combat.</param>
        /// <param name="enemy">The enemy that the player engaged.</param>
        public Combat(GameMap map, Player player, Enemy enemy)
        {
            InitializeComponent();

            _player = player;
            _enemy = enemy;
            _map = map;

            DisplayAllStats();
        }

        /// <summary>
        /// Handler for the attack button click. Tries to hurt the enemy, updates the display and lets the enemy take it's action.
        /// </summary>
        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            _enemy.Hurt(_player.Strength);
            DisplayAllStats();
            System.Threading.Thread.Sleep(TURN_DELAY);
            EnemyAction(_enemy, _player);
        }

        /// <summary>
        /// Handler for the block button click. Boosts the player's defense, updates the display and lets the enemy take it's action.
        /// </summary>
        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            _player.BlockMultiplier = BLOCK_MULTIPLIER;
            DisplayAllStats();
            System.Threading.Thread.Sleep(TURN_DELAY);
            EnemyAction(_enemy, _player);
        }

        /// <summary>
        /// Handler for the Inventory button. Opens an inventory window and closes this one.
        /// </summary>
        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            Inventory inv = new Inventory(_map, _player, _enemy);
            inv.Show();
            this.Close();
        }

        /// <summary>
        /// Handler for the Run Away button. Opens a new Game windows that uses the current player and map, then closes this window.
        /// </summary>
        private void RunAwayButton_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(_player, _map);
            game.Show();
            this.Close();
        }

        /// <summary>
        /// Displays all the stats and image of the player and enemy in their corresponding boxes.
        /// </summary>
        private void DisplayAllStats()
        {
            PlayerStats.Text = _player.AllStats;
            EnemyStats.Text = _enemy.AllStats;

            PlayerImg.Height = PlayerImg.Width;
            EnemyImg.Height = EnemyImg.Width;

            PlayerImg.Source = new BitmapImage(new Uri(Game.MapCharToImage(_player.GetMapDisplayChar()), UriKind.Relative));
            EnemyImg.Source = new BitmapImage(new Uri(Game.MapCharToImage(_enemy.GetMapDisplayChar()), UriKind.Relative));
        }

        // The Enemy "AI". Checks if dead, randomly chooses between attacking and blocking and checks if the player is dead.
        private void EnemyAction(Enemy enemy, Player player)
        {
            if (enemy.IsDead)
            {
                _map.RemoveThing(enemy, enemy.CurrentX, enemy.CurrentY);
                RunAwayButton_Click(null, null);
            }

            enemy.BlockMultiplier = 1;
            Random rng = new Random();

            if (rng.Next(2) == 0)
            {
                player.Hurt(enemy.Strength);
            }
            else
            {
                enemy.BlockMultiplier = BLOCK_MULTIPLIER;
            }

            player.BlockMultiplier = 1;
            DisplayAllStats();
            if (player.IsDead)
            {
                GameOver gameOverScreen = new GameOver(false);
                gameOverScreen.Show();
                this.Close();
            }
        }
    }
}
