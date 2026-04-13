using System.Collections.ObjectModel;
using System.Windows.Media;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Carousel;
using CineTrack.ViewModels.Favoris;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Protocols;

namespace CineTrack.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // to add more, refer to the official github https://github.com/Ervie/jikan.net/blob/master/JikanDotNet/Enumerations/AnimeGenreSearch.cs#L8
    private static readonly List<(string Name, int Id)> Genres = new()
    {
        ("Action", 1),
        ("Adventure", 2),
        ("Fantasy", 10),
        ("Romance", 22),
        ("Sci-Fi", 24),
        ("Mystery", 7)
    };

    private readonly IJikanService _jikanService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private bool _isLoading = true;
    public ObservableCollection<CarouselViewModel> Carousels { get; } = new();

    public MainViewModel(INavigationService navigationService, IJikanService jikanService)
    {
        _navigationService = navigationService;
        _jikanService = jikanService;
    }

    [RelayCommand]
    private void Disconnect()
    {
        SessionManager.Instance.CloseSession();
        _navigationService.NavigateTo<SignInViewModel>();
    }

    [RelayCommand]
    private void Favoris()
    {
        _navigationService.NavigateTo<FavorisViewModel>();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (Carousels.Count > 0) return; // skip the process if its already loaded

        IsLoading = true;
        Carousels.Clear();


        var trendingCarousel = new CarouselViewModel { GenreName = "Trending" };
        Carousels.Add(trendingCarousel);

        try {
            var trendingAnimes = await _jikanService.GetTrendingAnimesAsync();

            trendingCarousel.Initialize(trendingAnimes, _navigationService, AnimeSortType.Trending);



        } catch (Exception ex) {
            /* skip genre on failure */
        }


        foreach (var (name, id) in Genres)
        {
            var carousel = new CarouselViewModel { GenreName = name };
            Carousels.Add(carousel);

            try
            {
                await Task.Delay(600); // respect Jikan's rate limit
                var animes = await _jikanService.GetAnimesByGenreAsync(id);

                carousel.Initialize(animes, _navigationService, AnimeSortType.None);

                carousel.Initialize(animes, _navigationService);

            }
            catch
            {
                /* skip genre on failure */
            }
            finally
            {
                carousel.IsLoading = false;
            }
        }

        IsLoading = false;
    }
}