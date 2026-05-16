using CineTrack.Data.Models;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.ViewModels.Profile;
using CommunityToolkit.Mvvm.Input;

namespace CineTrack.ViewModels.AnimeDetails
{
    public partial class CommentaireViewModel
    {
        private readonly Commentaire _commentaire;
        private readonly int? _currentUserId;
        private readonly bool _isAdmin;
        private readonly INavigationService _navigationService;
        private readonly long _sourceMalId;

        public int Id => _commentaire.Id;
        public string Texte => _commentaire.Texte;
        public DateTime DateCreation => _commentaire.DateCreation;
        public int UtilisateurId => _commentaire.UtilisateurId;
        public bool IsCurrentUserAuthor => _currentUserId.HasValue &&
                                           _commentaire.UtilisateurId == _currentUserId.Value;
        public string Username { get; }
        public int? Rating { get; }
        public string RatingDisplay => Rating.HasValue ? $"★ {Rating}/5" : "";
        public bool CanDelete => _isAdmin || IsCurrentUserAuthor;

        public CommentaireViewModel(
            Commentaire commentaire,
            int? currentUserId,
            int? rating,
            bool isAdmin,
            INavigationService navigationService,
            long sourceMalId)
        {
            _commentaire = commentaire;
            _currentUserId = currentUserId;
            _isAdmin = isAdmin;
            _navigationService = navigationService;
            _sourceMalId = sourceMalId;
            Username = commentaire.Utilisateur?.Username ?? "Utilisateur inconnu";
            Rating = rating;
        }

        [RelayCommand]
        private void OpenProfile()
        {
            _navigationService.NavigateTo<ProfileViewModel>(new ProfileNavParam
            {
                UserId = _commentaire.UtilisateurId,
                Source = "animeDetails",
                SourceMalId = _sourceMalId
            });
        }
    }
}
