using CineTrack.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using JikanDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.ViewModels.AnimeDetails;
using CommunityToolkit.Mvvm.Input;

namespace CineTrack.ViewModels.AnimeCard
{
    public partial class AnimeCardViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly Anime _anime;

        // Data from Jikan
        public string Title => _anime.Title;
        public string? ImageUrl => _anime.Images?.JPG?.ImageUrl;
        public long? MalId => _anime.MalId;

        // Our Data
        [ObservableProperty]
        private double? _communityScore;

        [ObservableProperty]
        private int _rank;

        [ObservableProperty]
        private bool _showRank = false;

        // utilise score de l'anime si le score communautaire n'est pas disponible
        public string CommunityScoreDisplay => (_communityScore ?? _anime.Score).HasValue
            ? $"★ {CommunityScore.Value:F1} / 5"
            : "No ratings yet";

        public AnimeCardViewModel(Anime anime, INavigationService navigationService)
        {
            _anime = anime;

            _communityScore = anime.Score.HasValue ? anime.Score.Value / 2: null; // Convert to 5-star scale
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void SelectAnime()
        {
            _navigationService.NavigateTo<AnimeDetailsViewModel>(MalId.Value);
        }
    }
}
