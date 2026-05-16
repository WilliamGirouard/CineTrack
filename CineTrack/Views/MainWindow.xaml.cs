using CineTrack.Data.Services.NavigationServ;
using System.Windows;

namespace CineTrack
{
    public partial class MainWindow : Window
    {
        public MainWindow(INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = navigationService;
        }
    }
}