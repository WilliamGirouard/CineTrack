using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.ViewModels.Carousel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanDotNet;
using System.Collections.ObjectModel;
using ITransferParameter = CineTrack.Services.Interfaces.ITransferParameter;

namespace CineTrack.ViewModels.AnimeDetails;

public partial class AnimeDetailsViewModel : ObservableObject, ITransferParameter
{
    private readonly IJikanService _jikanService;
    private readonly INavigationService _navigationService;

    private Anime? _anime;
    private long? _malId;

    [ObservableProperty]
    private bool _isLoading = true;

    // Data from Jikan
    [ObservableProperty]
    private string? _title;
    
    [ObservableProperty]
    private string? _imageUrl;
    
    [ObservableProperty]
    private string? _desc;
    
    [ObservableProperty]
    private string? _episodes;
    
    [ObservableProperty]
    private string? _ageRating;

    // Our Data
    [ObservableProperty]
    private double? _communityScore;

    public string CommunityScoreDisplay => CommunityScore.HasValue
        ? $"★ {CommunityScore.Value:F1} / 5"
        : "No ratings yet";

    public AnimeDetailsViewModel(INavigationService navigationService, IJikanService jikanService)
    {
        _navigationService = navigationService;
        _jikanService = jikanService;
    }

    public void ReceiveAnimeId(long malId)
    {
        _malId = malId;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;

        // get the data
        _anime = await _jikanService.GetAnimeByIdAsync((int)_malId);

        // set the properties
        Title = _anime.Title;
        ImageUrl = _anime.Images?.JPG?.ImageUrl;
        Desc = _anime.Synopsis;
        Episodes = _anime.Episodes.HasValue ? $"{_anime.Episodes} episodes" : "Unknown episodes";
        AgeRating = _anime.Rating ?? "No rating";

        IsLoading = false;
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigationService.NavigateTo<MainViewModel>();
    }

    public void TransferParameter(object param)
    {
        if (param is long malId)
        {
            _malId = malId;
        }
    }
}