using CineTrack.Data.Models;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.Services.Jikan;
using CineTrack.ViewModels.AnimeDetails;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class FavoriteCardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public long MalId { get; }

    [ObservableProperty]
    private string title = "Loading...";

    [ObservableProperty]
    private string? imageUrl;

    public FavoriteCardViewModel(Favoris fav, INavigationService navigationService, IJikanService jikanService)
    {
        MalId = fav.MalId;
        _navigationService = navigationService;

        _ = LoadAsync(jikanService);
    }

    private async Task LoadAsync(IJikanService jikanService)
    {
        var anime = await jikanService.GetAnimeByIdAsync((int)MalId);

        if (anime == null)
        {
            title = "Not found";
            return;
        }

        Title = anime.Title;
        ImageUrl = anime.Images?.JPG?.ImageUrl;
    }

    [RelayCommand]
    private void Open()
    {
        _navigationService.NavigateTo<AnimeDetailsViewModel>(MalId);
    }
}