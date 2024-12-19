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
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void BtnExecute_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBack_Clicked(object sender, RoutedEventArgs e)
        {
            

            this.Close();

        }

        private void LbItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
