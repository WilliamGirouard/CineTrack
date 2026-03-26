using CineTrack.Data.Repositories.Interfaces;
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

        [ObservableProperty]
        private ObservableCollection<ModelFavoris> _favorisList = new();

        [ObservableProperty]
        private bool _isLoading = false;

        public FavorisViewModel(IFavorisRepository favorisRepository)
        {
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
    }
}