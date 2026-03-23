using CommunityToolkit.Mvvm.ComponentModel;

namespace CineTrack.Services
{
    /// <summary>
    /// Contrat du service de navigation entre ViewModels.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>ViewModel actuellement affiché dans le ContentControl.</summary>
        ObservableObject CurrentView { get; }

        /// <summary>Navigue vers le ViewModel de type T.</summary>
        void NavigateTo<T>() where T : ObservableObject;
    }
}
