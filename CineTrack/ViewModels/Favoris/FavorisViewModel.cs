using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services.Interfaces;
using CineTrack.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ModelFavoris = CineTrack.Data.Models.Favoris;

namespace CineTrack.ViewModels.Favoris
{
    public partial class FavorisViewModel : ObservableObject
    {
        private readonly IFavorisRepository _favorisRepository;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<ModelFavoris> _favorisList = new();

        [ObservableProperty]
        private bool _isLoading = false;

        public FavorisViewModel(IFavorisRepository favorisRepository, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _favorisRepository = favorisRepository;
            _= LoadFavorisAsync();
        }

        private async Task LoadFavorisAsync()
        {
            IsLoading = true;
            try
            {
                var currentUser = SessionManager.Instance.CurrentUser;
                if (currentUser == null)
                {
                    FavorisList = new ObservableCollection<ModelFavoris>();
                    return;
                }
                var userId = currentUser.Id;
                var favoris = await _favorisRepository.GetFavorisByUserIdAsync(userId);
                FavorisList = new ObservableCollection<ModelFavoris>(favoris);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RemoveFavoris(ModelFavoris favoris)
        {
            await _favorisRepository.RemoveFavorisAsync(favoris.Id);
            FavorisList.Remove(favoris);
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadFavorisAsync();
        }

        [RelayCommand]
        private void Retour()
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
    }
}