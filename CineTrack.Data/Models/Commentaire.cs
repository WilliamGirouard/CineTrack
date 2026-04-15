using JikanDotNet;
using System;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTrack.Data.Models
{
    public class Commentaire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AnimeId { get; set; }

        [Required]
        public int UtilisateurId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Texte { get; set; }

        [Required]
        public DateTime DateCreation { get; set; }

        [Required]
        public bool IsCurrentUserAuthor { get; set; }



        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur Utilisateur { get; set; }

        public Commentaire()
        {
            DateCreation = DateTime.Now;
        }
    }
}
