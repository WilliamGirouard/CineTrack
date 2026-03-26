using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class UtilisateurAnime
    {
        public int UtilisateurId { get; set; }
        public int AnimeId { get; set; }

        [Range(1, 5)]
        public int? Note { get; set; }

        public string? Commentaire { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur? Utilisateur { get; set; }

        [ForeignKey(nameof(AnimeId))]
        public Anime? Anime { get; set; }
    }
}
