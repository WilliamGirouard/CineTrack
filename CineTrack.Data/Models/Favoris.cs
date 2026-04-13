using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class Favoris
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UtilisateurId { get; set; }

        [Required]
        public int AnimeId { get; set; }

        //[Required]
        //public string ImageUrl { get; set; }

        //[Required]
        //public string TitreAnime { get; set; }

    }
}
