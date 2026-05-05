using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.Session;
using CineTrack.ViewModels.AnimeCard;
using CineTrack.ViewModels.AnimeDetails;
using CineTrack.ViewModels.Auth.Verification;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CineTrack.ViewModels.Profile;

public partial class ProfileViewModel : ObservableObject, ITransferParameter
{
    private readonly INavigationService _navigationService;
    private readonly IUtilisateurRepository _utilisateurRepository;
    private readonly IFavorisRepository _favorisRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IJikanService _jikanService;

    // Navigation back-state
    private string _source = "main";
    private long? _sourceMalId;

    [ObservableProperty] private bool _isLoading = true;
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _memberSince = string.Empty;
    [ObservableProperty] private int _reviewCount;
    [ObservableProperty] private bool _isOwnProfile;
    [ObservableProperty] private bool _isVerified;
    [ObservableProperty] private string _verificationMessage = string.Empty;

    public string UsernameInitial => Username.Length > 0 ? Username[0].ToString() : "?";

    partial void OnUsernameChanged(string value) => OnPropertyChanged(nameof(UsernameInitial));

    public ObservableCollection<AnimeCardViewModel> FavorisList { get; } = new();
    public ObservableCollection<RatedAnimeCardViewModel> RatingsList { get; } = new();

    public ProfileViewModel(
        INavigationService navigationService,
        IUtilisateurRepository utilisateurRepository,
        IFavorisRepository favorisRepository,
        INoteRepository noteRepository,
        IJikanService jikanService)
    {
        _navigationService = navigationService;
        _utilisateurRepository = utilisateurRepository;
        _favorisRepository = favorisRepository;
        _noteRepository = noteRepository;
        _jikanService = jikanService;
    }

    // Called by NavigationService when a ProfileNavParam is passed
    public void TransferParameter(object param)
    {
        if (param is ProfileNavParam navParam)
        {
            _source = navParam.Source;
            _sourceMalId = navParam.SourceMalId;
            _ = LoadAsync(navParam.UserId);
        }
        else
        {
            // No param provided — fall back to the current user's own profile
            _ = LoadOwnProfileAsync();
        }
    }

    // Called directly (e.g. "Mon profil" button) — loads current user's own profile
    [RelayCommand]
    public async Task LoadOwnProfileAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        if (currentUser == null) return;
        _source = "main";
        _sourceMalId = null;
        await LoadAsync(currentUser.Id);
    }

    private async Task LoadAsync(int userId)
    {
        IsLoading = true;
        FavorisList.Clear();
        RatingsList.Clear();


        try
        {
            var currentUser = SessionManager.Instance.CurrentUser;
            IsOwnProfile = currentUser?.Id == userId;

            // Resolve the target user
            Utilisateur? user = (IsOwnProfile && currentUser != null) ? currentUser : await _utilisateurRepository.GetUtilisateurByIdAsync(userId);

            if (user == null)
            {
                IsLoading = false; return;
            }

            Username = user.Username;
            IsVerified = user.UserVerified;

            if (IsOwnProfile)
            {
                VerificationMessage = IsVerified ? "Verified Account" : "Unverified Account";
            }
            
            MemberSince = user.DateCreation.ToString("MMMM yyyy");


            // Ratings section (shown for all profiles)
            var notes = await _noteRepository.GetNotesByUserIdAsync(userId);
            await LoadRatingsAsync(notes);

            // Favorites section (own profile only)
            if (IsOwnProfile)
            {
                await LoadFavorisAsync(userId);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ProfileViewModel.LoadAsync error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadRatingsAsync(List<Note> notes)
    {
        var semaphore = new SemaphoreSlim(3);
        var tasks = notes.Select(async note =>
        {
            await semaphore.WaitAsync();
            try
            {
                var anime = await _jikanService.GetAnimeByIdAsync(note.MalId);
                return (anime, note.NoteUtilisateur);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(tasks);
        foreach (var (anime, rating) in results)
        {
            RatingsList.Add(new RatedAnimeCardViewModel(anime, rating, _navigationService, source: "profile"));
        }
    }

    private async Task LoadFavorisAsync(int userId)
    {
        var favoris = await _favorisRepository.GetFavorisByUserIdAsync(userId);
        var semaphore = new SemaphoreSlim(3);

        var tasks = favoris.Select(async f =>
        {
            await semaphore.WaitAsync();
            try { return await _jikanService.GetAnimeByIdAsync(f.MalId); }
            finally { semaphore.Release(); }
        });

        var animes = await Task.WhenAll(tasks);
        foreach (var anime in animes)
        {
            FavorisList.Add(new AnimeCardViewModel(anime, _navigationService) { Source = "profile" });
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (_source == "animeDetails" && _sourceMalId.HasValue)
        {
            _navigationService.NavigateTo<AnimeDetailsViewModel>(new AnimeNavParam
            {
                MalId = _sourceMalId.Value,
                Source = "main"
            });
        }
        else
        {
            _navigationService.NavigateTo<MainViewModel>();
        }
    }

    [RelayCommand]
    
    private async Task VerifyAccountAsync()
    {
        var currentUser = SessionManager.Instance.CurrentUser;
        if(currentUser == null) return;
        _navigationService.NavigateTo<EmailVerificationViewModel>(new EmailVerificationParam
        {
            Email = currentUser.Email
        });
    }
}
