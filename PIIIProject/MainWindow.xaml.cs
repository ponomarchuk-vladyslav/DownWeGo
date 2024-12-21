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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Btn_StartClicked(object sender, RoutedEventArgs e)
        {
            Game game = new Game();

            game.Show();

            this.Close();

        }

        private void Btn_LoadClicked(object sender, RoutedEventArgs e)
        {

            //OpenFileDialog openFileDialog = new OpenFileDialog();

            Player tempP = null;
            GameMap tempGM = null;

            SaverLoader.Load(ref tempP, ref tempGM);

            Game game = new Game(tempP, tempGM);
            game.Show();
            this.Close();
        }

        private void Btn_InfoClicked(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Show();
            this.Close();

        }
    }
}