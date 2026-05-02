using CineTrack.ViewModels.Favoris;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        private async void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is FavorisViewModel vm)
                await vm.LoadFavorisAsync();
        }
    }
}
