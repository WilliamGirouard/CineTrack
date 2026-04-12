using CineTrack.Data.Models;
using CineTrack.ViewModels.AnimeCard;
using CineTrack.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanDotNet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace CineTrack.ViewModels.Carousel
{
    public enum AnimeSortType
    {
        Trending,
        None
    }
    public partial class CarouselViewModel : ObservableObject
    {
        private const int CardWidth = 150; // must match the Width in XAML
        private const int CardMargin = 12; // both sides of Margin="6,0"

        private int _availableWidth;

        // How many cards are visible at once
        private int AmountOfAnimes => _availableWidth > 0
            ? Math.Max(1, _availableWidth / (CardWidth + CardMargin))
            : 5;

        [ObservableProperty]
        private string _genreName = "";

        [ObservableProperty]
        private bool _isLoading = true;

        [ObservableProperty]
        private bool _showRank = false;


        private readonly List<AnimeCardViewModel> _allAnimes = new();

        private readonly ObservableCollection<AnimeCardViewModel> _rank = new();

        // Starting index
        private int _offset = 0;
        public ObservableCollection<AnimeCardViewModel> VisibleAnimes { get; } = new();

        // Called by MainViewModel after all animes are added

        public void Initialize(IEnumerable<JikanDotNet.Anime> animes, INavigationService navigationService, AnimeSortType sortType = AnimeSortType.None)
        {
            _allAnimes.Clear();
            ShowRank = sortType == AnimeSortType.Trending;

            var sortedData = sortType switch
            {
                AnimeSortType.Trending => animes.OrderByDescending(a => a.Score ?? 0),
                _ => animes
            };

            int currentRank = 1;

            foreach (var anime in sortedData)
            {
                var animeCard = new AnimeCardViewModel(anime, navigationService)
                {
                    Rank = ShowRank ? currentRank++ : 0,
                    ShowRank = ShowRank
                };
                _allAnimes.Add(animeCard);
            }

            _offset = 0;
            IsLoading = false;
            RefreshVisible();
        }

        [RelayCommand]
        private void Next()
        {
            if (_allAnimes.Count == 0) return;
            _offset = (_offset + 1) % _allAnimes.Count;
            RefreshVisible();
        }

        [RelayCommand]
        private void Previous()
        {
            if (_allAnimes.Count == 0) return;
            _offset = (_offset - 1 + _allAnimes.Count) % _allAnimes.Count;
            RefreshVisible();
        }

        public void UpdateWidth(double width)
        {
            _availableWidth = (int)width;
            RefreshVisible();
        }

        private void RefreshVisible()
        {
            VisibleAnimes.Clear();
            int count = _allAnimes.Count;
            if (count == 0) return;

            for (int i = 0; i < AmountOfAnimes; i++)
            {
                int index = (_offset + i) % count;
                VisibleAnimes.Add(_allAnimes[index]);
            }
        }
    }
}