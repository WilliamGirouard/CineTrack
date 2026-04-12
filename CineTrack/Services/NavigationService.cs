using CineTrack.Services.Jikan;
using CineTrack.ViewModels.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

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

        public void NavigateTo<T>(long param) where T : ObservableObject, IRequiresJikanData
        {
            var vm = _serviceProvider.GetRequiredService<T>(); // Gets the view model

            if (vm is IRequiresJikanData receiver) // if the view model needs data from Jikan...
            {
                receiver.ReceiveAnimeId(param); // ...it will get the data 
            }

            CurrentView = vm;
        }

    }
}
