using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Carousel;
using CineTrack.ViewModels.Favoris;
using CineTrack.ViewModels.Search;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;

namespace CineTrack.ViewModels;

public partial class MainViewModel : ObservableObject
{
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

    [ObservableProperty] private bool isLoading = true;
    [ObservableProperty] private string searchText = string.Empty;

    public ObservableCollection<CarouselViewModel> Carousels { get; } = new();
    public ObservableCollection<SearchAnimeCardViewModel> SearchResults { get; } = new();

    private CancellationTokenSource? _searchCts;

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
    private void OpenSearch()
    {
        _navigationService.NavigateTo<SearchViewModel>();
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = DebouncedSearch(value);
    }

    private async Task DebouncedSearch(string query)
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();

        try
        {
            await Task.Delay(300, _searchCts.Token);

            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                SearchResults.Clear();
                return;
            }

            var results = await _jikanService.SearchAnimeAsync(query);

            SearchResults.Clear();

            foreach (var anime in results.Take(8))
            {
                SearchResults.Add(new SearchAnimeCardViewModel(anime, _navigationService));
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (Carousels.Count > 0) return;

        IsLoading = true;
        Carousels.Clear();

        var trending = new CarouselViewModel { GenreName = "Trending" };
        Carousels.Add(trending);

        try
        {
            var trendingAnimes = await _jikanService.GetTrendingAnimesAsync();
            trending.Initialize(trendingAnimes, _navigationService, AnimeSortType.Trending);
        }
        catch { }

        foreach (var (name, id) in Genres)
        {
            var carousel = new CarouselViewModel { GenreName = name };
            Carousels.Add(carousel);

            try
            {
                await Task.Delay(600);
                var animes = await _jikanService.GetAnimesByGenreAsync(id);
                carousel.Initialize(animes, _navigationService);
            }
            catch { }
            finally
            {
                carousel.IsLoading = false;
            }
        }

        IsLoading = false;
    }
}