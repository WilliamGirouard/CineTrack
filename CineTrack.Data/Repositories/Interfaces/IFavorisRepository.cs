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
        Task<List<Favoris>> GetFavorisByUserIdAsync(int userId);
        Task AddFavorisAsync(Favoris favoris);
        Task RemoveFavorisAsync(int Id);
    }
}
