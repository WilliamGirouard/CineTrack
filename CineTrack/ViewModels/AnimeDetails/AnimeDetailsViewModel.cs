using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
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
    private readonly IFavorisRepository _favorisRepository;

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

    [ObservableProperty]
    private bool _isFavorite;

    partial void OnIsFavoriteChanged(bool value)
    {
        OnPropertyChanged(nameof(FavoriteButtonText));
    }

    public string FavoriteButtonText =>
    IsFavorite ? "Retirer des favoris ❤️" : "Ajouter aux favoris 🤍";

    public string CommunityScoreDisplay => CommunityScore.HasValue
        ? $"★ {CommunityScore.Value:F1} / 5"
        : "No ratings yet";

    public AnimeDetailsViewModel(INavigationService navigationService, IJikanService jikanService, IFavorisRepository favorisRepository)
    {
        _navigationService = navigationService;
        _jikanService = jikanService;
        _favorisRepository = favorisRepository;
    }

    public void ReceiveAnimeId(long malId)
    {
        _malId = malId;
    }

    private void CheckFavorite()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue)
            return;

        var fav = _favorisRepository.GetFavorisByUserId(user.Id);

        IsFavorite = fav.Any(f => f.AnimeId == _malId.Value);
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
        CommunityScore = _anime.Score.HasValue ? _anime.Score.Value / 2: null;
        OnPropertyChanged(nameof(CommunityScoreDisplay));

        CheckFavorite();

        IsLoading = false;
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue)
            return;

        try
        {
            if (IsFavorite)
            {
                var existing = _favorisRepository
                    .GetFavorisByUserId(user.Id)
                    .FirstOrDefault(f => f.AnimeId == _malId.Value);

                if (existing != null)
                    _favorisRepository.RemoveFavoris(existing.Id);

                IsFavorite = false;
            }
            else
            {
                var fav = new CineTrack.Data.Models.Favoris
                {
                    UtilisateurId = user.Id,
                    AnimeId = (int)_malId.Value
                };

                _favorisRepository.AddFavoris(fav);

                IsFavorite = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Favorite error: {ex.Message}");
        }
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