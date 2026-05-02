using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.AdminServ
{
    public interface IAdminService
    {
        Task ElevateUserToAdminAsync(int userId);
        Task DeleteUserAsync(int userId);
        Task DeleteCommentaireAsync(int commentaireId);
        Task<Boolean> IsAdminAsync(int userId);
        Task DeleteNoteAsync(int noteId);
    }
}
