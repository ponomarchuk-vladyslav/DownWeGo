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
using Microsoft.Win32;

namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        // Constants for the game map size
        const int GAMEMAP_COLUMNS = 27;
        const int GAMEMAP_ROWS = 16;

        // private fields
        private GameMap _map;
        private Player _player;

        /// <summary>
        /// Default constructor. Called on a new game. Constructs a new map.
        /// </summary>
        public Game()
        {
            InitializeComponent();

            try
            {
                _map = new GameMap(GAMEMAP_ROWS, GAMEMAP_COLUMNS);
                _player = Player.NewPlayer(_map, 24, 2);

                ConstructMap();

                InitializeBackground();
                UpdateDisplay(_map, _player);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Warning! Error has occured while creating map:\n{ex.Message}", "Error");
                this.Close();
            }
        }

        /// <summary>
        /// Constructor called with the ready map and player pointer provided to it.
        /// </summary>
        /// <param name="player">Player pointer provided.</param>
        /// <param name="map">Map provided.</param>
        public Game(Player player, GameMap map)
        {
            InitializeComponent();

            _map = map;
            _player = player;

            InitializeBackground();
            UpdateDisplay(_map, _player);
        }

        /// <summary>
        /// Processes the keypresses of the user and move the player character accordingly. Also deals with any collisions.
        /// </summary>
        /// <param name="e">The key pressed by the player.</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ICollidable contact;
            switch (e.Key)
            {
                case Key.W:
                    contact = _player.MovePlayer(GameMap.Direction.Up, _map);
                    break;
                case Key.S:
                    contact = _player.MovePlayer(GameMap.Direction.Down, _map);
                    break;
                case Key.D:
                    contact = _player.MovePlayer(GameMap.Direction.Right, _map);
                    break;
                case Key.A:
                    contact = _player.MovePlayer(GameMap.Direction.Left, _map);
                    break;
                default:
                    contact = null;
                    break;
            }

            // If the player walks into an enemy, open combat screen and close this one.
            if (contact is Enemy)
            {
                Combat combatScreen = new Combat(_map, _player, contact as Enemy);
                combatScreen.Show();
                SaverLoader.Save(_player, _map);
                this.Close();
            }

            // If the player walks into an exit and all the enemies are dead, open gameover screen and close this one.
            if (contact is Escape && Enemy.EnemyCount <= 0)
            {
                GameOver gameOverScreen = new GameOver(true);
                gameOverScreen.Show();
                this.Close();
            }

            UpdateDisplay(_map, _player);
        }

        /// <summary>
        /// Converts a character to a corresponding image path using a switch statement.
        /// I made this because I originaly used characters for display, this helps to map pictures on top of them.
        /// </summary>
        /// <param name="c">The character to convert.</param>
        /// <returns>A path to the corresponding picture.</returns>
        public static string MapCharToImage(char c)
        {
            string img;

            switch (c)
            {
                case Item.ITEM_DISPLAY_CHAR:
                    img = @"\Sprites\Chest.png";
                    break;
                case Player.PLAYER_DISPLAY_CHAR:
                    img = @"\Sprites\knight.png";
                    break;
                case Wall.WALL_DISPLAY_CHAR:
                    img = @"\Sprites\wall.png";
                    break;
                case Enemy.ENEMY_DISPLAY_CHAR:
                    img = @"\Sprites\skeleton.png";
                    break;
                // This returns an empty path because I added a background of floor tiles/
                case GameMap.FLOOR_DISPLAY_CHAR:
                    img = "";
                    break;
                case Enemy.BOSS_DISPLAY_CHAR:
                    img = @"\Sprites\Boss.png";
                    break;
                case Escape.ESCAPE_DISPLAY_CHAR:
                    img = @"\Sprites\Escape.png";
                    break;
                default:
                    img = @"\Sprites\Wall.png";
                    break;
            }

            return img;
        }

        /// <summary>
        /// Updates the display. Centers the display on the position of the player.
        /// </summary>
        /// <param name="map">The map to display.</param>
        /// <param name="player">The player to center on.</param>
        private void UpdateDisplay(GameMap map, Player player)
        {
            if (player is null)
                throw new ArgumentNullException("The player is null");
            if (map is null)
                throw new ArgumentNullException("The map is null.");

            // Calculates the offset (distance from player to the display edge if the player is in the middle of the display
            int heightDifference = (MapDisplay.Rows / 2) + player.CurrentY;
            int widthDifference = (MapDisplay.Columns / 2) - player.CurrentX;

            char[,] mapDisplay = map.DisplayMap;
            string path;

            // Clears the map
            MapDisplay.Children.Clear();

            // Go through all the cells of the map display
            for (int row = 0; row < MapDisplay.Rows; row++)
            {
                for (int column = 0; column < MapDisplay.Columns; column++)
                {
                    // Create new image
                    Image img = new Image();

                    // If the tile to be displayed is not in the map (the player is near the edge of the map, and the display is supposed to show stuff beyond the map).
                    if (heightDifference - row < 0 || column < widthDifference || heightDifference - row >= mapDisplay.GetLength(0) || column - widthDifference >= mapDisplay.GetLength(1))
                    {
                        // Displays an empty image
                        path = MapCharToImage(' ');
                    }
                    else
                    {
                        // Displays the corresponding image
                        path = MapCharToImage(mapDisplay[heightDifference - row, column - widthDifference]);
                    }
                    img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                    img.Stretch = Stretch.Fill;

                    // Adds the image to the display grid
                    MapDisplay.Children.Add(img);
                }
            }

            // Updates the health and enemy count display
            HealthDisplay.Text = $"Health: {player.Health}";
            EnemyCountDisplay.Text = $"Enemies left: {Enemy.EnemyCount}";
        }

        /// <summary>
        /// Makes sure that the background is the same size as the display itself and fills it with floor pictures.
        /// </summary>
        private void InitializeBackground()
        {
            Background.Rows = MapDisplay.Rows;
            Background.Columns = MapDisplay.Columns;

            for (int row = 0; row < Background.Rows; row++)
            {
                for (int column = 0; column < Background.Columns; column++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"\Sprites\Floor.png", UriKind.Relative));
                    img.Stretch = Stretch.Fill;
                    img.Margin = new Thickness(0.05);

                    Background.Children.Add(img);
                }
            }
        }

        /// <summary>
        /// Handles the inventory button click. Opens a new inventory window and closes this one.
        /// </summary>
        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory(_map, _player);
            inventory.Show();
            this.Close();
        }

        /// <summary>
        /// Creates an example map.
        /// </summary>
        private void ConstructMap()
        {
            // Adds the border walls
            _map.AddWall(0, 0, 0, 15);
            _map.AddWall(0, 15, 26, 15);
            _map.AddWall(26, 15, 26, 0);
            _map.AddWall(26, 0, 0, 0);

            // Adds different walls
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

            // Adds health potions
            _map.AddThing(new HealthPotion(), 24, 12);
            _map.AddThing(new HealthPotion(), 10, 6);
            _map.AddThing(new HealthPotion(), 1, 2);
            _map.AddThing(new HealthPotion(), 19, 5);
            _map.AddThing(new HealthPotion(), 25, 1);
            _map.AddThing(new HealthPotion(), 3, 7);

            // Adds enemies
            Enemy.NewEnemy(_map, 3, 2, 3);
            Enemy.NewEnemy(_map, 19, 6, 3);
            Enemy.NewEnemy(_map, 24, 10, 5);
            Enemy.NewEnemy(_map, 15, 8, 3);
            Enemy.NewEnemy(_map, 8, 5, 10);

            // Adds some strength and defense potions
            _map.AddThing(new StrengthPotion(), 14, 12);
            _map.AddThing(new DefensePotion(), 14, 12);
            _map.AddThing(new StrengthPotion(), 5, 12);
            _map.AddThing(new DefensePotion(), 24, 8);
            _map.AddThing(new StrengthPotion(), 15, 2);
            _map.AddThing(new DefensePotion(), 25, 1);

            // Adds an escape
            _map.AddThing(new Escape(), 25, 14);
        }

        /// <summary>
        /// Method that handles the "Save" button. Saves the game when pressed.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string saveLocation = null;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Save Files |*.save";

            if (saveFileDialog.ShowDialog() == true)
            {
                saveLocation = saveFileDialog.FileName;
            }

            if (!string.IsNullOrEmpty(saveLocation))
            {
                try
                {
                    SaverLoader.Save(_player, _map, saveLocation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Warning! Error has occured while trying to save file:\n{ex.Message}", "Error");
                }
            }
        }
    }
}
