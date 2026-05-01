using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.Carousel;
using CineTrack.ViewModels.Favoris;
using CineTrack.ViewModels.Watch;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ITransferParameter = CineTrack.Services.Interfaces.ITransferParameter;
using JikanAnime = JikanDotNet.Anime;

namespace CineTrack.ViewModels.AnimeDetails;

public partial class AnimeDetailsViewModel : ObservableObject, ITransferParameter
{
    private readonly IJikanService _jikanService;
    private readonly INavigationService _navigationService;
    private readonly IFavorisRepository _favorisRepository;
    private readonly ICommentaireRepository _commentaireRepository;

    private JikanAnime? _anime;
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

    [ObservableProperty]
    private ObservableCollection<Commentaire> _commentaires = new();

    [ObservableProperty]
    private string _newComment = string.Empty;

    // Liste des épisodes pour la simulation de visionnage
    [ObservableProperty]
    private ObservableCollection<int> _episodeList = new();

    // Source de navigation (main ou favoris)
    private string _source = "main";

    partial void OnIsFavoriteChanged(bool value)
    {
        OnPropertyChanged(nameof(FavoriteButtonText));
    }

    public string FavoriteButtonText =>
    _isFavorite ? "Retirer des favoris ❤️" : "Ajouter aux favoris 🤍";

    public string CommunityScoreDisplay => CommunityScore.HasValue
        ? $"★ {CommunityScore.Value:F1} / 5"
        : "No ratings yet";

    public AnimeDetailsViewModel(INavigationService navigationService, IJikanService jikanService, IFavorisRepository favorisRepository, ICommentaireRepository commentaireRepository)
    {
        _navigationService = navigationService;
        _jikanService = jikanService;
        _favorisRepository = favorisRepository;
        _commentaireRepository = commentaireRepository;
    }

    // Vérifie si l'anime est dans les favoris de l'utilisateur
    private async Task CheckFavoriteAsync()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue)
            return;

        var fav = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);

        IsFavorite = fav.Any(f => f.AnimeId == (int)_malId.Value);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;

        if (!_malId.HasValue)
        {
            IsLoading = false;
            return; // ou gérer l'erreur / charger un état "vide"
        }

        // get the data
        _anime = await _jikanService.GetAnimeByIdAsync((int)_malId.Value);

        if (_anime == null)
        { 
            IsLoading = false;
            return;
        }

        // set the properties
        Title = _anime.Title;
        ImageUrl = _anime.Images?.JPG?.ImageUrl;
        Desc = _anime.Synopsis;
        Episodes = _anime.Episodes.HasValue ? $"{_anime.Episodes} episodes" : "Unknown episodes";
        AgeRating = _anime.Rating ?? "No rating";
        CommunityScore = _anime.Score.HasValue ? _anime.Score.Value / 2: null;
        OnPropertyChanged(nameof(CommunityScoreDisplay));

        // Génère la liste des épisodes
        if (_anime.Episodes.HasValue)
        {
            EpisodeList.Clear();
            for (int i = 1; i <= _anime.Episodes.Value; i++)
            {
                EpisodeList.Add(i);
            }

        }

        Debug.WriteLine($"Loading anime with ID: {_malId}");
        await CheckFavoriteAsync();
        await LoadCommentsAsync();

        IsLoading = false;
    }

    // Navigue vers la page de visionnage de l'épisode sélectionné
    [RelayCommand]
    private async Task WatchEpisodeDetails(int episodeNumber)
    {
        _navigationService.NavigateTo<WatchViewModel>(new WatchNavParam
        {
            MalId = _malId!.Value,
            EpisodeNumber = episodeNumber,
            Source = _source
        });
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue)
            return;

        try
        {
            var favorisList = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);
            var existing = favorisList.FirstOrDefault(f => f.AnimeId == (int)_malId.Value);

            if (existing != null)
            {
                await _favorisRepository.RemoveFavorisAsync(existing.Id);
                IsFavorite = false;
            }
            else
            {
                var fav = new CineTrack.Data.Models.Favoris
                {
                    UtilisateurId = user.Id,
                    AnimeId = (int)_malId.Value
                };

                await _favorisRepository.AddFavorisAsync(fav);
                IsFavorite = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }



    // Retourne vers la page précédente selon la source de navigation
    [RelayCommand]
    private async Task GoBack()
    {
        if (_source == "favoris")
            _navigationService.NavigateTo<FavorisViewModel>();
        else
            _navigationService.NavigateTo<MainViewModel>();
    }


   public void TransferParameter(object param)
{
    if (param is AnimeNavParam navParam)
    {
        _malId = navParam.MalId;
        _source = navParam.Source;
    }
    else if (param is long id)
    {
        _malId = id;
        _source = "main";
    }
    else if (param is int idInt)
    {
        _malId = idInt;
        _source = "main";
    }

    _ = LoadAsync();
}

    private async Task LoadCommentsAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;

        try
        {
            var commentaires = await _commentaireRepository.GetCommentairesByAnimeIdAsync((int)_anime.MalId);

            Commentaires.Clear();
            foreach (var commentaire in commentaires)
            {
                commentaire.IsCurrentUserAuthor = (currentUser != null && commentaire.UtilisateurId == currentUser.Id);
                Commentaires.Add(commentaire);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreurs lors du chargement des commentaires : {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AddCommentsAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser == null || string.IsNullOrEmpty(NewComment) || _anime is null)
        {
            return;
        }

        try
        {
            var commentaire = new Commentaire
            {
                AnimeId = (int)_anime.MalId,
                UtilisateurId = currentUser.Id,
                Texte = NewComment,
                DateCreation = DateTime.Now,
                IsCurrentUserAuthor = true
            };

            await _commentaireRepository.AddCommentaireAsync(commentaire);
            NewComment = string.Empty;
            await LoadCommentsAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de l'ajout du commentaire : {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteCommentsAsync(Commentaire commentaire)
    {
        Console.WriteLine($"Tentative de suppression du commentaire ID: {commentaire.Id}, Auteur ID: {commentaire.UtilisateurId}"); 

        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser == null || commentaire.UtilisateurId != currentUser.Id)
            return;

        try
        {
            await _commentaireRepository.RemoveCommentaireAsync(commentaire.Id);
            await LoadCommentsAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de la suppression du commentaire : {ex.Message}");
        }
    }
}