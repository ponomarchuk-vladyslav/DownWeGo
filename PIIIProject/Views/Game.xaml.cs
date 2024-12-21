using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using System.IO;

namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        const int Columns = 27;
        const int Rows = 16;

        private GameMap _map;
        private Player _player;

        public Game()
        {
            InitializeComponent();

            _map = new GameMap(Rows, Columns);
            _player = new Player(0, 7);
            _map.AddThing(_player, _player.CurrentX, _player.CurrentY);

            for (int i = 0; i < 5; i++)
            {
                _map.AddThing(new Wall(), 5, 5 + i);
            }

            _map.AddThing(new HealthPotion(), 7, 7);
            _map.AddThing(new HealthPotion(), 7, 7);
            _map.AddThing(new HealthPotion(), 3, 2);
            _map.AddThing(new HealthPotion(), 15, 5);

            _map.AddThing(new Enemy(3), 10, 10);

            _map.AddWall(25, 15, 20, 3);

            UpdateDisplay(_map, _player);
        }

        public Game(Player player, GameMap map)
        {
            InitializeComponent();

            _map = map;
            _player = player;

            UpdateDisplay(_map, _player);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Enemy enemyContact;
            switch (e.Key)
            {
                case Key.W:
                    enemyContact = _player.MovePlayer(GameMap.Direction.Up, _map);
                    break;
                case Key.S:
                    enemyContact = _player.MovePlayer(GameMap.Direction.Down, _map);
                    break;
                case Key.D:
                    enemyContact = _player.MovePlayer(GameMap.Direction.Right, _map);
                    break;
                case Key.A:
                    enemyContact = _player.MovePlayer(GameMap.Direction.Left, _map);
                    break;
                default:
                    enemyContact = null;
                    break;
            }

            if (enemyContact is not null)
            {
                Combat combatScreen = new Combat(_player, enemyContact);
                combatScreen.Show();
                SaverLoader.Save(_player, _map);
                this.Close();
            }

            UpdateDisplay(_map, _player);
        }

        public string MapCharToImage(char c)
        {
            string img;

            switch (c)
            {
                case Item.ITEM_DISPLAY_CHAR:
                    img = "./Sprites/Chest.png";
                    break;
                case Player.PLAYER_DISPLAY_CHAR:
                    img = "./Sprites/knight.jpg";
                    break;
                case Wall.WALL_DISPLAY_CHAR:
                    img = "./Sprites/wall.png";
                    break;
                case Enemy.ENEMY_DISPLAY_CHAR:
                    img = "./Sprites/skeleton.png";
                    break;
                default:
                    img = "./Sprites/wall.png";
                    break;
            }

            return img;
        }

        public void UpdateDisplay(GameMap map, Player player)
        {
            int heightDifference = player.CurrentY - (Rows / 2);
            int widthDifference = (Columns / 2) - player.CurrentX;
            char[,] mapDisplay = map.DisplayMap;
            string path;

            Map.Children.Clear();

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    TextBlock txt = new TextBlock()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 20
                    };

                    if (row < heightDifference || column < widthDifference || row - heightDifference >= mapDisplay.GetLength(0) || column - widthDifference >= mapDisplay.GetLength(1))
                    {
                        txt.Text = "";
                    }
                    else
                    {
                        txt.Text = mapDisplay[row - heightDifference, column - widthDifference].ToString();
                    }
                    //Image img = new Image();
                    //path = MapCharToImage(mapDisplay[i, j]);
                    //img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                    //Map.Children.Add(img);
                    Map.Children.Add(txt);
                }
            }
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory(_player);

            inventory.Show();
        }
    }
}
