using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.AnimeCard;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CineTrack.ViewModels.Favoris;

public partial class FavorisViewModel : ObservableObject
{
    private readonly IFavorisRepository _favorisRepository;
    private readonly INavigationService _navigationService;
    private readonly IJikanService _jikanService;

    [ObservableProperty]
    private ObservableCollection<AnimeCardViewModel> _favorisList = new();

    [ObservableProperty]
    private bool _isLoading;

    public FavorisViewModel(
        IFavorisRepository favorisRepository,
        INavigationService navigationService,
        IJikanService jikanService)
    {
        _favorisRepository = favorisRepository;
        _navigationService = navigationService;
        _jikanService = jikanService;

        _ = LoadFavorisAsync();
    }

    public async Task LoadFavorisAsync()
    {
        IsLoading = true;

        try
        {
            var user = SessionManager.Instance.CurrentUser;

            if (user == null)
            {
                _favorisList.Clear();
                return;
            }

            var favoris = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);

            var list = new List<AnimeCardViewModel>();

            foreach (var fav in favoris)
            {
                var anime = await _jikanService.GetAnimeByIdAsync((int)fav.AnimeId);
                if (anime == null) continue;

                list.Add(new AnimeCardViewModel(anime, _navigationService));
            }

            FavorisList.Clear();
            foreach (var item in list)
                FavorisList.Add(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RemoveFavoris(AnimeCardViewModel card)
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null) return;

        var favorisList = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);
        var fav = favorisList.FirstOrDefault(f => f.AnimeId == card.MalId);

        if (fav == null) return;

        await _favorisRepository.RemoveFavorisAsync(fav.Id);
        _favorisList.Remove(card);
    }

    [RelayCommand]
    private void Retour()
    {
        _navigationService.NavigateTo<MainViewModel>();
    }
}