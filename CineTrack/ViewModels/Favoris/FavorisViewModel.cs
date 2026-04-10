using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
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
            LoadFavoris();
        }

        private void LoadFavoris()
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
                var favoris = _favorisRepository.GetFavorisByUserId(userId);
                FavorisList = new ObservableCollection<ModelFavoris>(favoris);
            }
            //Bro avait pas mis le catch sa faisait crash
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
        private void RemoveFavoris(ModelFavoris favoris)
        {
            _favorisRepository.RemoveFavoris(favoris.Id);
            FavorisList.Remove(favoris);
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadFavoris();
        }

        [RelayCommand]
        private void Retour()
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
    }
}