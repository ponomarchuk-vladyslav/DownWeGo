using Microsoft.Win32;
using PIIIProject.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIIIProject.Models;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Default constructor. Everything starts here.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Handles the start button click. Starts a new game "from scratch" and closes this window.
        /// </summary>
        private void Btn_StartClicked(object sender, RoutedEventArgs e)
        {
            Game game = new Game();

            game.Show();

            this.Close();

        }

        /// <summary>
        /// Loads a game from a file, if possible.
        /// </summary>
        private void Btn_LoadClicked(object sender, RoutedEventArgs e)
        {
            Player tempP = null;
            GameMap tempGM = null;
            string saveLocation = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Save Files |*.save";


            if (openFileDialog.ShowDialog() == true)
            {
                saveLocation = openFileDialog.FileName;
            }

            if (saveLocation is not null)
            {
                try
                {
                    SaverLoader.Load(ref tempP, ref tempGM, saveLocation);

                    Game game = new Game(tempP, tempGM);
                    game.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Warning! Error has occured while trying to load file:\n{ex.Message}", "Error");
                }
            }
        }

        /// <summary>
        /// Handles the info button click. Opens an info window and closes this one.
        /// </summary>
        private void Btn_InfoClicked(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Show();
            this.Close();
        }
    }
}