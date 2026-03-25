using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class Favoris
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; }
        public int AnimeId { get; set; }

        public required string TitreAnime { get; set; }

    }
}
