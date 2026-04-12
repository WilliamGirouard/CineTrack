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

namespace CineTrack.Views
{
    /// <summary>
    /// Interaction logic for T_A.xaml
    /// </summary>
    public partial class T_C : Window
    {
        public T_C()
        {
            InitializeComponent();
        }
        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
