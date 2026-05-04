using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.NoteServ;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.Profile;
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
    private readonly INoteService _noteService;

    private JikanAnime? _anime;
    private long? _malId;

    // Source de navigation (main ou profile)
    private string _source = "main";

    private const int LimiteEpisodeParPage = 50;

    [ObservableProperty] private bool _isLoading = false;

    // Data from Jikan
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _imageUrl;
    [ObservableProperty] private string? _desc;
    [ObservableProperty] private string? _episodes;
    [ObservableProperty] private string? _ageRating;
    [ObservableProperty] private string? _jikanScore;

    // Our Data
    [ObservableProperty] private double? _communityScore;
    [ObservableProperty] private bool _isFavorite;
    [ObservableProperty] private int? _userRating;
    [ObservableProperty] private ObservableCollection<CommentaireViewModel> _commentaires = new();
    [ObservableProperty] private string _newComment = string.Empty;

    [ObservableProperty] private int _currentEpisodePage = 1;
    // Liste des épisodes pour la simulation de visionnage
    [ObservableProperty] private ObservableCollection<int> _episodeList = new();

    public int TotalEpisodePages => _anime?.Episodes.HasValue == true ? (int )Math.Ceiling(_anime.Episodes.Value / (double)LimiteEpisodeParPage) : 1;
    partial void OnIsFavoriteChanged(bool value) => OnPropertyChanged(nameof(FavoriteButtonText));
    partial void OnUserRatingChanged(int? value) => OnPropertyChanged(nameof(CommunityScoreDisplay));

    public string FavoriteButtonText => IsFavorite ? "Retirer des favoris ❤️" : "Ajouter aux favoris 🤍";

    public string CommunityScoreDisplay => CommunityScore.HasValue
        ? $"★ {CommunityScore.Value:F1} / 5"
        : "No ratings yet";

    public bool HasEpisodes => _anime?.Episodes.HasValue == true && _anime.Episodes > 0;

    public AnimeDetailsViewModel(
        INavigationService navigationService,
        IJikanService jikanService,
        IFavorisRepository favorisRepository,
        ICommentaireRepository commentaireRepository,
        INoteService noteService)
    {
        _navigationService = navigationService;
        _jikanService = jikanService;
        _favorisRepository = favorisRepository;
        _commentaireRepository = commentaireRepository;
        _noteService = noteService;
    }

    // Vérifie si l'anime est dans les favoris de l'utilisateur
    private async Task CheckFavoriteAsync()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue) return;
        var fav = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);
        IsFavorite = fav.Any(f => f.MalId == (int)_malId.Value);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        if (!_malId.HasValue) { IsLoading = false; return; }

        var currentUser = SessionManager.Instance.CurrentUser;

        // Optimisation : Jikan lent donc lance tout parallelement
        var animeTask =  _jikanService.GetAnimeByIdAsync((int)_malId.Value);
        var CommunityScoreTask =  _noteService.GetCommunityScoreAsync(_malId.Value);

        _anime = await animeTask;
        if (_anime == null) { IsLoading = false; return; }

        // set the properties
        Title = _anime.Title;
        ImageUrl = _anime.Images?.JPG?.ImageUrl;
        Desc = _anime.Synopsis;
        Episodes = _anime.Episodes.HasValue ? $"{_anime.Episodes} episodes" : "Unknown episodes";
        AgeRating = _anime.Rating ?? "No rating";
        JikanScore = _anime.Score.HasValue ? $"★ {_anime.Score.Value / 2:F1} / 5" : "No score yet";

        //Affiche les images, titres etc pdt que l'info charge
        IsLoading = false;

        // Génère la liste des épisodes
        if (_anime.Episodes.HasValue)
        {
            LoadEpisodePage();
        }

        // Load this user's existing rating
        CommunityScore = await CommunityScoreTask;
        // Community score from our DB, not Jikan

        OnPropertyChanged(nameof(CommunityScoreDisplay));

        if (currentUser != null)
            UserRating = await _noteService.GetNoteAsync(currentUser.Id, _malId.Value);

        await CheckFavoriteAsync();
        await LoadCommentsAsync();
    }

    // Navigue vers la page de visionnage de l'épisode sélectionné
    [RelayCommand]
    private void WatchEpisodeDetails(int episodeNumber)
    {
        _navigationService.NavigateTo<WatchViewModel>(new WatchNavParam
        {
            MalId = _malId!.Value,
            EpisodeNumber = episodeNumber,
            Source = _source
        });
    }

    [RelayCommand]
    private async Task RateAsync(object parameter)
    {
        if (parameter is not string s || !int.TryParse(s, out int rating)) return;

        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue) return;

        await _noteService.RateAsync(user.Id, _malId.Value, rating);

        UserRating = rating;
        CommunityScore = await _noteService.GetCommunityScoreAsync(_malId.Value);
        OnPropertyChanged(nameof(CommunityScoreDisplay));

        await LoadCommentsAsync();
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue) return;

        try
        {
            var favorisList = await _favorisRepository.GetFavorisByUserIdAsync(user.Id);
            var existing = favorisList.FirstOrDefault(f => f.MalId == (int)_malId.Value);

            if (existing != null)
            {
                await _favorisRepository.RemoveFavorisAsync(existing.Id);
                IsFavorite = false;
            }
            else
            {
                await _favorisRepository.AddFavorisAsync(new CineTrack.Data.Models.Favoris
                {
                    UtilisateurId = user.Id,
                    MalId = (int)_malId.Value
                });
                IsFavorite = true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
    }

    // Retourne vers la page précédente selon la source de navigation
    [RelayCommand]
    private void GoBack()
    {
        if (_source == "profile")
        {
            var currentUser = SessionManager.Instance.CurrentUser;
            if (currentUser != null)
            {
                _navigationService.NavigateTo<ProfileViewModel>(new ProfileNavParam
                {
                    UserId = currentUser.Id,
                    Source = "main"
                });
            }
            else
            {
                _navigationService.NavigateTo<MainViewModel>();
            }
        }
        else
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
    }

    // Reçoit les paramètres de navigation
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
        IsLoading = true;
        _ = LoadAsync();
    }

    private async Task LoadCommentsAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        var isAdmin = currentUser?.Role == EnumRole.admin;
        try
        {
            var commentaires = await _commentaireRepository.GetCommentairesByAnimeIdAsync((long)_anime!.MalId);
            Debug.WriteLine($"Commentaires : {commentaires.Count}");
            Commentaires.Clear();
            foreach (var commentaire in commentaires)
            {
                var rating = await _noteService.GetNoteAsync(commentaire.UtilisateurId, (long)_anime.MalId);
                Commentaires.Add(new CommentaireViewModel(
                    commentaire,
                    currentUser?.Id,
                    rating,
                    isAdmin,
                    _navigationService,
                    (long)_anime.MalId));
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
        if (currentUser == null || string.IsNullOrEmpty(NewComment) || _anime is null) return;
        Debug.WriteLine($"MalId utilisé: {(int)_anime.MalId}");
        try
        {
            await _commentaireRepository.AddCommentaireAsync(new Commentaire
            {
                MalId = (int)_anime.MalId,
                UtilisateurId = currentUser.Id,
                Texte = NewComment,
                DateCreation = DateTime.Now,
            });
            NewComment = string.Empty;
            await LoadCommentsAsync();
        }
        catch (Exception ex) { Debug.WriteLine($"Erreur lors de l'ajout du commentaire : {ex.Message} \n InnerMessage : {ex.InnerException?.Message}\n{ex.InnerException?.InnerException?.Message}"); }
    }

    [RelayCommand]
    private async Task DeleteCommentsAsync(CommentaireViewModel commentaire)
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser == null) return;

        if (commentaire.UtilisateurId != currentUser.Id && currentUser.Role != EnumRole.admin) return;

        try
        {
            await _commentaireRepository.RemoveCommentaireAsync(commentaire.Id);
            await LoadCommentsAsync();
        }
        catch (Exception ex) { Debug.WriteLine($"Erreur lors de la suppression du commentaire : {ex.Message}"); }
    }

    private void LoadEpisodePage()
    {
        if (_anime?.Episodes == null || _anime.Episodes == 0)
        {
            EpisodeList.Clear();
            return;
        }

        EpisodeList.Clear();
        int start = (CurrentEpisodePage - 1) * LimiteEpisodeParPage + 1;
        int end = Math.Min(CurrentEpisodePage * LimiteEpisodeParPage, _anime.Episodes.Value);

        for (int i = start; i <= end; i++)
        {
            EpisodeList.Add(i);
        }
        OnPropertyChanged(nameof(TotalEpisodePages));
        OnPropertyChanged(nameof(HasEpisodes));
    }

    [RelayCommand]
    private void NextEpisodePage()
    {
        if (CurrentEpisodePage < TotalEpisodePages)
        {
            CurrentEpisodePage++;
            LoadEpisodePage();
        }
    }

    [RelayCommand]
    private void PreviousEpisodePage()
    {
        if (CurrentEpisodePage > 1)
        {
            CurrentEpisodePage--;
            LoadEpisodePage();
        }
    }
}
