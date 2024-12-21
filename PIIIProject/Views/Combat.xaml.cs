using System;
using System.Collections.Generic;
using System.Linq;
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
        const double BLOCK_MULTIPLIER = 1.5;

        private Player _player;
        private Enemy _enemy;

        private int _playerDefenseMult;
        private int _enemyDefenseMult;

        public Combat(Player player, Enemy enemy)
        {
            InitializeComponent();

            _player = player;
            _enemy = enemy;
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            _enemy.Hurt(_player.Strength);
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            _player.BlockMultiplier = BLOCK_MULTIPLIER;
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            Inventory inv = new Inventory(_player);
            inv.Show();
        }

        private void RunAwayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DisplayAllStats(Player player, Enemy enemy)
        {

        }

        private void EnemyAction(Enemy enemy, Player player)
        {
            enemy.BlockMultiplier = 1;
            Random rng = new Random();

            if (rng.Next(1) == 0)
            {
                enemy.Hurt(player.Strength);
            }
            else
            {
                enemy.BlockMultiplier = BLOCK_MULTIPLIER;
            }

            player.BlockMultiplier = 1;
            
        }
    }
}
