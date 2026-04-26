using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.NoteServ;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JikanAnime = JikanDotNet.Anime;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ITransferParameter = CineTrack.Services.Interfaces.ITransferParameter;

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

    [ObservableProperty] private bool _isLoading = true;

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

    partial void OnIsFavoriteChanged(bool value) => OnPropertyChanged(nameof(FavoriteButtonText));
    partial void OnUserRatingChanged(int? value) => OnPropertyChanged(nameof(CommunityScoreDisplay));

    public string FavoriteButtonText => _isFavorite ? "Retirer des favoris ❤️" : "Ajouter aux favoris 🤍";

    public string CommunityScoreDisplay => CommunityScore.HasValue
        ? $"★ {CommunityScore.Value:F1} / 5"
        : "No ratings yet";

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

        _anime = await _jikanService.GetAnimeByIdAsync(_malId.Value);

        Title = _anime.Title;
        ImageUrl = _anime.Images?.JPG?.ImageUrl;
        Desc = _anime.Synopsis;
        Episodes = _anime.Episodes.HasValue ? $"{_anime.Episodes} episodes" : "Unknown episodes";
        AgeRating = _anime.Rating ?? "No rating";
        JikanScore = _anime.Score.HasValue ? $"★ {_anime.Score.Value / 2:F1} / 5" : "No score yet";

        // Community score from our DB, not Jikan
        CommunityScore = _noteService.GetCommunityScore(_malId.Value);
        OnPropertyChanged(nameof(CommunityScoreDisplay));

        // Load this user's existing rating
        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser != null)
            UserRating = _noteService.GetNote(currentUser.Id, _malId.Value);

        await CheckFavoriteAsync();
        await LoadCommentsAsync();

        IsLoading = false;
    }

    [RelayCommand]
    private async Task RateAsync(object parameter)
    {
        if (parameter is not string s || !int.TryParse(s, out int rating)) return;

        var user = SessionManager.Instance.CurrentUser;
        if (user == null || !_malId.HasValue) return;

        await _noteService.RateAsync(user.Id, _malId.Value, rating);

        UserRating = rating;
        CommunityScore = _noteService.GetCommunityScore(_malId.Value);
        OnPropertyChanged(nameof(CommunityScoreDisplay));

        // Refresh comments so rating badge updates
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

    [RelayCommand]
    private void GoBack() => _navigationService.NavigateTo<MainViewModel>();

    public void TransferParameter(object param)
    {
        if (param is long id) _malId = id;
        else if (param is int idInt) _malId = idInt;
        _ = LoadAsync();
    }

    private async Task LoadCommentsAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        try
        {
            var commentaires = await _commentaireRepository.GetCommentairesByAnimeIdAsync((long)_anime!.MalId);
            Commentaires.Clear();
            foreach (var commentaire in commentaires)
            {
                var rating = _noteService.GetNote(commentaire.UtilisateurId, (long)_anime.MalId);
                Commentaires.Add(new CommentaireViewModel(commentaire, currentUser?.Id, rating));
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
        catch (Exception ex) { Debug.WriteLine($"Erreur lors de l'ajout du commentaire : {ex.Message}"); }
    }

    [RelayCommand]
    private async Task DeleteCommentsAsync(CommentaireViewModel commentaire)
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser == null || commentaire.UtilisateurId != currentUser.Id) return;

        try
        {
            await _commentaireRepository.RemoveCommentaireAsync(commentaire.Id);
            await LoadCommentsAsync();
        }
        catch (Exception ex) { Debug.WriteLine($"Erreur lors de la suppression du commentaire : {ex.Message}"); }
    }
}