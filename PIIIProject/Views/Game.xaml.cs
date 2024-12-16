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

namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        const int Columns = 27;
        const int Rows = 16;
        const int Increment = 1;

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

            int[,] map = new int[Columns, Rows];
            //{
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { }, 
            //    { },
            //    { }, 
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { }, 
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { },
            //    { }
            //};

            Dictionary<int, string> spriteNumbers = new Dictionary<int, string>
            {
                {Floor, "/Sprites/Floor.png"},
                {Wall, "/Sprites/wall.png"},
                {Player, "/Sprites/knight.jpg"},
                {Item, "/Sprites/Chest.png"},
                {Enemy, "/Sprites/skeleton.png"},
                {Boss, "/Sprites/Boss.png"},
                {Escape, "/Sprites/Escape.png"}

            };
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int sprite = map[i, j]; //Finds the integer corresponding to the coords
                    string path = spriteNumbers[sprite]; //Takes the corresponding string and whatever cause dictionaries are cool

                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(path, UriKind.Relative)), //Allows relative paths
                        Stretch = Stretch.Fill //Fills the whole space
                    };

                    Map.Children.Add(image);
                }
            }

            
        }

    }
}
