using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTrack.Data.Models
{
    public class UtilisateurAnime
    {
        public int UtilisateurId { get; set; }

        public int MalId { get; set; }

        [Range(1, 5)]
        public int? Note { get; set; }

        public string? Commentaire { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur? Utilisateur { get; set; }
    }
}