using CineTrack.Data.Services.NavigationServ;
using CineTrack.ViewModels.AnimeDetails;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanDotNet;

namespace CineTrack.ViewModels.Profile;

public partial class RatedAnimeCardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly long _malId;

    public string Title { get; }
    public string? ImageUrl { get; }
    public int UserRating { get; }
    public string RatingDisplay => $"★ {UserRating} / 5";
    public string Source { get; }

    public RatedAnimeCardViewModel(Anime anime, int userRating, INavigationService navigationService, string source = "main")
    {
        _navigationService = navigationService;
        _malId = (long) anime.MalId;
        Title = anime.Title;
        ImageUrl = anime.Images?.JPG?.ImageUrl;
        UserRating = userRating;
        Source = source;
    }

    [RelayCommand]
    private void Open()
    {
        _navigationService.NavigateTo<AnimeDetailsViewModel>(new AnimeNavParam
        {
            MalId = _malId,
            Source = Source
        });
    }
}
