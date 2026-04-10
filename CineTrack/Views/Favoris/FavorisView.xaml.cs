using CineTrack.ViewModels.Favoris;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;


namespace CineTrack.Views.Favoris
{
    /// <summary>
    /// Interaction logic for FavorisView.xaml
    /// </summary>
    public partial class FavorisView : UserControl
    {
        public FavorisView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<FavorisViewModel>();
        }

    }
}
