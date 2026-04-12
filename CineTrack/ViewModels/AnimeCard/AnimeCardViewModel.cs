using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JikanDotNet;

namespace CineTrack.ViewModels.AnimeCard
{
    public partial class AnimeCardViewModel : ObservableObject
    {
        private readonly Anime _anime;

        // Data from Jikan
        public string Title => _anime.Title;
        public string? ImageUrl => _anime.Images?.JPG?.ImageUrl;
        public long? MalId => _anime.MalId;

        // Our Data
        [ObservableProperty]
        private double? _communityScore;

        public string CommunityScoreDisplay => CommunityScore.HasValue
            ? $"★ {CommunityScore.Value:F1} / 5"
            : "No ratings yet";

        public AnimeCardViewModel(Anime anime)
        {
            _anime = anime;
        }
    }
}
