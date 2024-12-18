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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        const int Columns = 27;
        const int Rows = 16;

        const int Floor = 0;
        const int Wall = 1;
        const int Player = 2;
        const int Item = 3;
        const int Enemy = 4;
        const int Boss = 5;
        const int Escape = 6;
        public Game()
        {
            InitializeComponent();

            GameMap map = new GameMap(Rows, Columns);

            //Dictionary<int, string> spriteNumbers = new Dictionary<int, string>
            //{
            //    {Floor, "/Sprites/Floor.png"},
            //    {Wall, "/Sprites/wall.png"},
            //    {Player, "/Sprites/knight.jpg"},
            //    {Item, "/Sprites/Chest.png"},
            //    {Enemy, "/Sprites/skeleton.png"},
            //    {Boss, "/Sprites/Boss.png"},
            //    {Escape, "/Sprites/Escape.png"}

            //};

            Player player = new Player(map, 0, 0);

            for (int i = 0; i < 5; i++)
            {
                map.AddThing(new Wall(), 5, 5 + i);
            }

            map.AddThing(new Item(), 7, 7);

            UpdateDisplay(map);
        }

        public string MapCharToImage(char c)
        {
            string img;

            switch (c)
            {
                case Models.Item.ITEM_DISPLAY_CHAR:
                    img = "/Sprites/Chest.png";
                    break;
                case Models.Player.PLAYER_DISPLAY_CHAR:
                    img = "/Sprites/knight.jpg";
                    break;
                case Models.Wall.WALL_DISPLAY_CHAR:
                    img = "/Sprites/wall.png";
                    break;
                default:
                    img = "/Sprites/Floor.png";
                    break;
            }

            return img;
        }

        public void UpdateDisplay(GameMap map)
        {
            char[,] mapDisplay = map.DisplayMap;
            string path;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    path = MapCharToImage(mapDisplay[i, j]);

                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(path, UriKind.Relative)), //Allows relative paths
                        Stretch = Stretch.Fill //Fills the whole space
                    };

                    Map.Children.Add(image);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.W)
            {

            }
            else if (e.Key == Key.D)
            {

            }
            else if(e.Key == Key.S)
            {

            }
            else if( e.Key == Key.A)
            {

            }
        }
    }
}
