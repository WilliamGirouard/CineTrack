using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        
        [Required] 
        [MaxLength(50)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,}$", 
            ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre, un caractère spécial et 10 caractères.")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        public DateTime DateCreation { get; set; }

        public Utilisateur()
        {
            DateCreation = DateTime.Now;
        }
    }
}
