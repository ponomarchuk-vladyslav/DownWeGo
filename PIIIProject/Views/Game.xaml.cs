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
        const int GAMEMAP_COLUMNS = 27;
        const int GAMEMAP_ROWS = 16;

        private GameMap _map;
        private Player _player;

        public Game()
        {
            InitializeComponent();

            _map = new GameMap(GAMEMAP_ROWS, GAMEMAP_COLUMNS);
            _player = new Player(22, 12);
            _map.AddThing(_player, _player.CurrentX, _player.CurrentY);

            for (int i = 0; i < 5; i++)
            {
                _map.AddThing(new Wall(), 5, 5 + i);
            }

            ConstructMap();

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
                    img = @"\Sprites\Chest.png";
                    break;
                case Player.PLAYER_DISPLAY_CHAR:
                    img = @"\Sprites\knight.jpg";
                    break;
                case Wall.WALL_DISPLAY_CHAR:
                    img = @"\Sprites\wall.png";
                    break;
                case Enemy.ENEMY_DISPLAY_CHAR:
                    img = @"\Sprites\skeleton.png";
                    break;
                case GameMap.FLOOR_DISPLAY_CHAR:
                    img = @"\Sprites\floor.png";
                    break;
                default:
                    img = @"\Sprites\Wall.png";
                    break;
            }

            return img;
        }

        public void UpdateDisplay(GameMap map, Player player)
        {
            int heightDifference = (Map.Rows / 2) + player.CurrentY;
            int widthDifference = (Map.Columns / 2) - player.CurrentX;
            char[,] mapDisplay = map.DisplayMap;
            string path;

            Map.Children.Clear();

            for (int row = 0; row < Map.Rows; row++)
            {
                for (int column = 0; column < Map.Columns; column++)
                {
                    Image img = new Image();

                    if (heightDifference - row < 0 || column < widthDifference || heightDifference - row >= mapDisplay.GetLength(0) || column - widthDifference >= mapDisplay.GetLength(1))
                    {
                        path = MapCharToImage('0');
                    }
                    else
                    {
                        path = MapCharToImage(mapDisplay[heightDifference - row, column - widthDifference]);
                    }
                    img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                    img.Stretch = Stretch.Fill;
                    img.Margin = new Thickness(0.05);

                    //TextBox img = new TextBox();
                    //img.Text = $"{column - widthDifference}, {heightDifference - row}";
                    Map.Children.Add(img);
                }
            }
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory(_player);

            inventory.Show();
        }

        private void ConstructMap()
        {

            _map.AddWall(0, 0, 0, 15);
            _map.AddWall(0, 15, 26, 15);
            _map.AddWall(26, 15, 26, 0);
            _map.AddWall(26, 0, 0, 0);

            _map.AddWall(7, 9, 7, 13);
            _map.AddWall(7, 9, 2, 9);
            _map.AddWall(7, 9, 2, 9);
            _map.AddWall(7, 9, 15, 9);
            _map.AddWall(15, 9, 20, 9);
            _map.AddWall(20, 9, 20, 13);
            _map.AddWall(20, 13, 25, 13);
            _map.AddWall(7, 9, 7, 4);
            _map.AddWall(4, 0, 4, 1);
            _map.AddWall(4, 3, 4, 4);
            _map.AddWall(4, 4, 0, 4);
            _map.AddWall(4, 4, 0, 4);
            _map.AddWall(22, 0, 22, 1);
            _map.AddWall(22, 3, 22, 4);
            _map.AddWall(22, 4, 23, 4);
            _map.AddWall(25, 4, 26, 4);
            _map.AddWall(22, 4, 22, 9);
            _map.AddWall(22, 9, 23, 9);
            _map.AddWall(25, 9, 26, 9);
            _map.AddWall(21, 4, 9, 4);
            _map.AddWall(16, 7, 16, 4);
            _map.AddWall(16, 7, 18, 7);
            _map.AddWall(21, 7, 20, 7);
            






            _map.AddThing(new HealthPotion(), 24, 12);
            _map.AddThing(new HealthPotion(), 10, 6);
            _map.AddThing(new HealthPotion(), 1, 2);
            _map.AddThing(new HealthPotion(), 19, 5);

            _map.AddThing(new Enemy(3), 3, 2);
            _map.AddThing(new Enemy(4), 19, 6);
            _map.AddThing(new Enemy(5), 24, 10);
            _map.AddThing(new Enemy(3), 15, 8);
            _map.AddThing(new Enemy(6), 1, 10);
            _map.AddThing(new Enemy(7), 8, 14);
            _map.AddThing(new Enemy(7), 8, 5);




        }
    }
}
