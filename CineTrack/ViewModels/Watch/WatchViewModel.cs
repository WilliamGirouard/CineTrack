using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.ViewModels.AnimeDetails;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using ITransferParameter = CineTrack.Services.Interfaces.ITransferParameter;

namespace CineTrack.ViewModels.Watch
{
    public partial class WatchViewModel : ObservableObject, ITransferParameter
    {
        private readonly IJikanService _jikanService;
        private readonly INavigationService _navigationService;

        private long? _malId;

        private string _source = "main";

        [ObservableProperty] 
        private string? _title;
        [ObservableProperty] 
        private string? _imageUrl;
        [ObservableProperty] 
        private int _episodeNumber;
        [ObservableProperty] 
        private int _totalEpisodes;
        [ObservableProperty] 
        private bool _isLoading = true;
        [ObservableProperty] 
        private double _progress = 0;
        [ObservableProperty] 
        private bool _isPlaying = false;
        [ObservableProperty] 
        private string _currentTime = "0:00";
        [ObservableProperty] 
        private string _totalTime = "24:00";

        public WatchViewModel(INavigationService navigationService, IJikanService jikanService)
        {
            _navigationService = navigationService;
            _jikanService = jikanService;
        }

        // Reçoit les paramètres de navigation
        public void TransferParameter(object param) 
        {
            if (param is WatchNavParam navParam)
            {
                _malId = navParam.MalId;
                EpisodeNumber = navParam.EpisodeNumber;
                _source = navParam.Source;
            }

            _ = LoadAsync();
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            if (!_malId.HasValue) return;

            IsLoading = true;

            // Récupère les données de l'anime depuis Jikan
            var anime = await _jikanService.GetAnimeByIdAsync((int)_malId.Value);

            if (anime == null)
            {
                IsLoading = false;
                return;
            }

            // Assigne les propriétés
            Title = anime.Title;
            ImageUrl = anime.Images?.JPG?.ImageUrl;
            TotalEpisodes = anime.Episodes ?? 0;

            IsLoading = false;
        }

        // Joue ou met en pause la simulation
        [RelayCommand]
        private async Task TogglePlay() => IsPlaying = !IsPlaying;

        // Passe à l'épisode suivant
        [RelayCommand]
        private async Task NextEpisode()
        {
            if (EpisodeNumber < TotalEpisodes)
            {
                EpisodeNumber++;
                Progress = 0;
            }
        }

        // Passe à l'épisode précédent
        [RelayCommand]
        private async Task PreviousEpisode()
        {
            if (EpisodeNumber > 1)
            {
                EpisodeNumber--;
                Progress = 0;
            }
        }

        // Retourne vers la page de détails de l'anime
        [RelayCommand]
        private async Task GoBack()
        {
            if (_malId.HasValue)
                _navigationService.NavigateTo<AnimeDetailsViewModel>(new AnimeNavParam
                {
                    MalId = _malId.Value,
                    Source = _source
                });
            else
                _navigationService.NavigateTo<MainViewModel>();
        }

    }
}
