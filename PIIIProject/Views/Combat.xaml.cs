using System;
using System.Collections.Generic;
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
        const double BLOCK_MULTIPLIER = 1.5;

        private GameMap _map;
        private Player _player;
        private Enemy _enemy;

        public Combat(GameMap map, Player player, Enemy enemy)
        {
            InitializeComponent();

            _player = player;
            _enemy = enemy;
            _map = map;
            DisplayAllStats(player, enemy);
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            _enemy.Hurt(_player.Strength);
            EnemyAction(_enemy, _player);
            DisplayAllStats(_player, _enemy);
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            _player.BlockMultiplier = BLOCK_MULTIPLIER;
            EnemyAction(_enemy, _player);
            DisplayAllStats(_player, _enemy);
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            Inventory inv = new Inventory(_player);
            inv.Show();
        }

        private void RunAwayButton_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(_player, _map);
            game.Show();
            this.Close();
        }

        private void DisplayAllStats(Player player, Enemy enemy)
        {
            PlayerStats.Text = player.AllStats;
            EnemyStats.Text = enemy.AllStats;
        }

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
            DisplayAllStats(player, enemy);
        }
    }
}
