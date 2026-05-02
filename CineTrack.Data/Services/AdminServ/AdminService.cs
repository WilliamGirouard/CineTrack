using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.AdminServ
{
    public class AdminService : IAdminService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly ICommentaireRepository _commentaireRepository;

        public AdminService(IUtilisateurRepository utilisateurRepository, ICommentaireRepository commentaireRepository)
        {
            _utilisateurRepository = utilisateurRepository;
            _commentaireRepository = commentaireRepository;
        }
        public async Task ElevateUserToAdminAsync(int userId)
        {
            var user = await _utilisateurRepository.GetUtilisateurByIdAsync(userId) ?? throw new Exception("User not found");
            if (user.Role == Models.EnumRole.admin) {
                throw new Exception($"User : {user.Id} is already an administrator.");
            }
            user.Role = Models.EnumRole.admin;
            await _utilisateurRepository.UpdateUserAsync(user);

        }
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _utilisateurRepository.GetUtilisateurByIdAsync(userId) ?? throw new Exception("User not found");
            if (user.Role == Models.EnumRole.admin)
            {
                throw new Exception($"User : {user.Id} is an administrator => account cannot be deleted.");
            }
            await _utilisateurRepository.DeleteUserAsync(user);
        }
        public async Task<Boolean> IsAdmin(int userId)
        {
            var user = await _utilisateurRepository.GetUtilisateurByIdAsync(userId) ?? throw new Exception("User not found");
            if (user.Role != Models.EnumRole.admin)
            {
                return false;
            }
            return true;
        }
        public async Task DeleteCommentaireAsync(int commentaireId)
        {
            var commentaire = await _commentaireRepository.GetCommentaireAsync(commentaireId) ?? throw new Exception($"Commentaire #{commentaireId} not found");
            if (await IsAdmin(commentaire.UtilisateurId))
            {
                throw new Exception($"Commentaire #{commentaireId} is an administrator's => commentaire cannot be deleted.");
            }
            await _commentaireRepository.RemoveCommentaireAsync(commentaireId);
        }
    }
}
