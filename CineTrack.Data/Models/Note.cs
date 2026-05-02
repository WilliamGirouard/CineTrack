using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UtilisateurId { get; set; }

        [Required]
        public long MalId { get; set; }

        [Required]
        [Range(1, 5)]
        public int NoteUtilisateur { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UtilisateurId))]
        public Utilisateur? Utilisateur { get; set; }
    }
}
