using CineTrack.ViewModels.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace CineTrack.Services
{
    /// La MainWindow contient un ContentControl bindé sur CurrentView.
    /// Un DataTemplate dans App.xaml mappe chaque ViewModel vers sa View.

    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private ObservableObject _currentView = null!;
        public ObservableObject CurrentView
        {
            get => _currentView;
            private set => SetProperty(ref _currentView, value);
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        ///Résout le ViewModel T via DI et l'affiche dans le ContentControl.
        public void NavigateTo<T>() where T : ObservableObject
        {
            CurrentView = _serviceProvider.GetRequiredService<T>();
        }
    }
}
