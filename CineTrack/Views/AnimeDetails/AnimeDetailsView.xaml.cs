using CineTrack.ViewModels.AnimeDetails;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CineTrack.Views.AnimeDetails
{
    /// <summary>
    /// Interaction logic for AnimeDetailsView.xaml
    /// </summary>
    public partial class AnimeDetailsView : UserControl
    {
        public AnimeDetailsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is AnimeDetailsViewModel vm && vm.LoadCommand.CanExecute(null))
                vm.LoadCommand.Execute(null);
        }
    }
}
