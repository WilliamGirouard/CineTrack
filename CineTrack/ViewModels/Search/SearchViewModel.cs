using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanDotNet;
using System.Collections.ObjectModel;

namespace CineTrack.ViewModels.Search;

public partial class SearchViewModel : ObservableObject
{
    private readonly IJikanService _jikanService;
    private readonly INavigationService _navigationService;

    public SearchViewModel(IJikanService jikanService, INavigationService navigationService)
    {
        _jikanService = jikanService;
        _navigationService = navigationService;
    }

    [ObservableProperty] private string searchText;
    [ObservableProperty] private bool onlyAiring;
    [ObservableProperty] private string minEpisodes;
    [ObservableProperty] private string maxEpisodes;

    public ObservableCollection<SearchAnimeCardViewModel> Results { get; } = new();

    private List<Anime> _baseResults = new();
    private string _lastQuery = "";

    partial void OnSearchTextChanged(string value)
    {
        _lastQuery = value ?? "";
        _ = DebouncedSearch(_lastQuery);
    }

    [RelayCommand]
    private void Retour()
    {
        _navigationService.NavigateTo<MainViewModel>();
    }

    private CancellationTokenSource _cts;

    private async Task DebouncedSearch(string query)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(400, _cts.Token);

            if (query != _lastQuery)
                return;

            var results = await _jikanService.SearchAnimeAsync(query);

            if (query != _lastQuery)
                return;

            _baseResults = results.ToList();
            ApplyFilters();
        }
        catch (TaskCanceledException)
        {
            // ignore
        }
    }

    partial void OnOnlyAiringChanged(bool value)
    {
        ReapplyOrSearch();
    }

    partial void OnMinEpisodesChanged(string value)
    {
        ReapplyOrSearch();
    }

    partial void OnMaxEpisodesChanged(string value)
    {
        ReapplyOrSearch();
    }

    private void ApplyFilters()
    {
        Results.Clear();

        foreach (var anime in _baseResults)
        {
            if (OnlyAiring && anime.Airing != true)
                continue;

            if (int.TryParse(MinEpisodes, out var min))
            {
                if (anime.Episodes.HasValue && anime.Episodes.Value < min)
                    continue;
            }

            if (int.TryParse(MaxEpisodes, out var max))
            {
                if (anime.Episodes.HasValue && anime.Episodes.Value > max)
                    continue;
            }

            Results.Add(new SearchAnimeCardViewModel(anime, _navigationService));
        }
    }

    private void ReapplyOrSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= 2)
        {
            _ = DebouncedSearch(SearchText);
            return;
        }

        ApplyFilters();
    }
}