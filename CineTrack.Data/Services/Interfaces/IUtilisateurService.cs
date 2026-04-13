using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.Interfaces
{
    public interface IUtilisateurService
    {
        Task SignUpAsync(string username, string fullName, string email, string password);
        Task<Utilisateur> SignInAsync(string username, string password);

        Task<bool> IsUsernameUsedAsync(string username);
        Task<bool> IsEmailUsedAsync(string email);
        Task<string> ForgottenPasswordAsync(string username);
        Task<bool> IsResetCodeValidAsync(string email, string code);

        Task ResetPasswordAsync(string email, string newPassword);
    }
}
