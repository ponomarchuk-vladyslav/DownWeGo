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

            _player = new Player(_map, 0, 7);

            for (int i = 0; i < 5; i++)
            {
                _map.AddThing(new Wall(), 5, 5 + i);
            }

            _map.AddThing(new HealthPotion(), 7, 7);

            UpdateDisplay(_map);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _player.MovePlayer(GameMap.Direction.Up, _map);
                    break;
                case Key.S:
                    _player.MovePlayer(GameMap.Direction.Down, _map);
                    break;
                case Key.D:
                    _player.MovePlayer(GameMap.Direction.Right, _map);
                    break;
                case Key.A:
                    _player.MovePlayer(GameMap.Direction.Left, _map);
                    break;
                default:
                    break;
            }

            UpdateDisplay(_map);
        }

        public string MapCharToImage(char c)
        {
            string img;

            switch (c)
            {
                case Models.Item.ITEM_DISPLAY_CHAR:
                    img = "./Sprites/Chest.png";
                    break;
                case Models.Player.PLAYER_DISPLAY_CHAR:
                    img = "./Sprites/knight.jpg";
                    break;
                case Models.Wall.WALL_DISPLAY_CHAR:
                    img = "./Sprites/wall.png";
                    break;
                default:
                    img = "./Sprites/Floor.png";
                    break;
            }

            return img;
        }

        public void UpdateDisplay(GameMap map)
        {
            char[,] mapDisplay = map.DisplayMap;
            string path;

            Map.Children.Clear();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    //Image img = new Image();
                    //path = MapCharToImage(mapDisplay[i, j]);
                    //img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                    //Map.Children.Add(img);
                    TextBlock txt = new TextBlock();
                    txt.Text = mapDisplay[i, j].ToString();
                    txt.HorizontalAlignment = HorizontalAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;
                    txt.FontSize = 20;

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
