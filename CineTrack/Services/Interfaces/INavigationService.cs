using CommunityToolkit.Mvvm.ComponentModel;

namespace CineTrack.Services.Interfaces
{

    /// Contrat du service de navigation entre ViewModels.

    public interface INavigationService
    {
        ///ViewModel actuellement affiché dans le ContentControl.
        ObservableObject CurrentView { get; }

        ///Navigue vers le ViewModel de type T.
        void NavigateTo<T>() where T : ObservableObject;
        // Navigue en envoyant un parametre dans un autre ViewModel (Utilise pour mdp oublie)
        void NavigateTo<T>(object param) where T : ObservableObject;
    }
}
