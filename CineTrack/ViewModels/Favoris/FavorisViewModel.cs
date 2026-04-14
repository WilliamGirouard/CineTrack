using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.AnimeCard;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
                FavorisList.Clear();
                return;
            }

            var favoris = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);

            var semaphore = new SemaphoreSlim(5);

            var tasks = favoris.Select(async f =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var anime = await _jikanService.GetAnimeByIdAsync((int)f.AnimeId);
                    return anime;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var animes = await Task.WhenAll(tasks);

            FavorisList.Clear();

            foreach (var anime in animes.Where(a => a != null))
            {
                FavorisList.Add(new AnimeCardViewModel(anime, _navigationService));
            }
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