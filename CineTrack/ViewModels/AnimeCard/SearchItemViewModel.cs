using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.ViewModels.AnimeDetails;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanDotNet;
using System.DirectoryServices;

namespace CineTrack.ViewModels.Search;

public partial class SearchAnimeCardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly Anime _anime;

    public string Title => _anime.Title;
    public string? Synopsis => _anime.Synopsis;
    public string? ImageUrl => _anime.Images?.JPG?.ImageUrl;
    public long? MalId => _anime.MalId;

    public SearchAnimeCardViewModel(Anime anime, INavigationService navigationService)
    {
        _anime = anime;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void Open()
    {
        if (MalId.HasValue)
        {
            _navigationService.NavigateTo<AnimeDetailsViewModel>(MalId.Value);
        }
    }
}