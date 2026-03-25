using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories.Interfaces
{
    public interface IFavorisRepository
    {
        List<Favoris> GetFavorisByUserId(int userId);
        void AddFavoris(Favoris favoris);
        void RemoveFavoris(int Id);
    }
}
