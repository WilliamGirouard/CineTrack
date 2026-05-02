using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Models;

namespace CineTrack.ViewModels.AnimeDetails
{
    public class CommentaireViewModel
    {
        private readonly Commentaire _commentaire;
        private readonly int? _currentUserId;
        private readonly bool _isAdmin;
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

        public CommentaireViewModel(Commentaire commentaire, int? currentUserId, int? rating, bool isAdmin)
        {
            _commentaire = commentaire;
            _currentUserId = currentUserId;
            _isAdmin = isAdmin;
            Username = commentaire.Utilisateur?.Username ?? "Utilisateur inconnu";
            Rating = rating;
        }
    }
}