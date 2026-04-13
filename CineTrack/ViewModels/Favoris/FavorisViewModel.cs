using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
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
    private ObservableCollection<AnimeCardViewModel> favorisList = new();

    [ObservableProperty]
    private bool isLoading;

    public FavorisViewModel(
        IFavorisRepository favorisRepository,
        INavigationService navigationService,
        IJikanService jikanService)
    {
        _favorisRepository = favorisRepository;
        _navigationService = navigationService;
        _jikanService = jikanService;
    }

    // IMPORTANT: call this when page opens
    [RelayCommand]
    public async Task LoadFavoris()
    {
        isLoading = true;

        try
        {
            var user = SessionManager.Instance.CurrentUser;
            if (user == null)
            {
                favorisList.Clear();
                return;
            }

            var favoris = _favorisRepository.GetFavorisByUserId(user.Id);

            var list = new List<AnimeCardViewModel>();

            foreach (var fav in favoris)
            {
                var anime = await _jikanService.GetAnimeByIdAsync(fav.AnimeId);
                if (anime == null) continue;

                list.Add(new AnimeCardViewModel(anime, _navigationService));
            }

            favorisList = new ObservableCollection<AnimeCardViewModel>(list);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            isLoading = false;
        }
    }

    [RelayCommand]
    private void RemoveFavoris(AnimeCardViewModel card)
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null) return;

        var fav = _favorisRepository
            .GetFavorisByUserId(user.Id)
            .FirstOrDefault(f => f.AnimeId == card.MalId);

        if (fav != null)
        {
            _favorisRepository.RemoveFavoris(fav.Id);
            favorisList.Remove(card);
        }
    }

    [RelayCommand]
    private void Retour()
    {
        _navigationService.NavigateTo<MainViewModel>();
    }
}