using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Services.EmailServ;
using CineTrack.Data.Services.HashServ;
using CineTrack.Data.Services.PasswordResetStoreServ;

namespace CineTrack.Data.Services.UtilisateurServ
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IEmailService _emailService;
        private readonly Dictionary<string, (string Code, DateTime Expiry)> _passwordResetCodes;

        public UtilisateurService(IUtilisateurRepository utilisateurRepository, IEmailService emailService, PasswordResetStore passwordResetStore)
        {
            _utilisateurRepository = utilisateurRepository;
            _emailService = emailService;
            _passwordResetCodes = passwordResetStore.Codes;
        }

        public async Task<bool> IsEmailUsedAsync(string email)
        {
            return await _utilisateurRepository.GetByEmailAsync(email) != null;
        }

        public async Task<bool> IsUsernameUsedAsync(string username)
        {
            return await _utilisateurRepository.GetByUsernameAsync(username) != null;
        }

        public async Task<Utilisateur> SignInAsync(string username, string password)
        {
            Utilisateur userVerif = await _utilisateurRepository.GetByUsernameAsync(username) ?? throw new Exception("Invalid credentials");
            if (!HashService.CompareHashToPassword(password, userVerif.Password))
            {
                throw new Exception("Invalid credentials");
            }
            return userVerif;
        }

        public async Task SignUpAsync(string username, string fullName, string email, string password)
        {
            if (await IsEmailUsedAsync(email))
            {
                throw new Exception("Email already used");
            }
            if (await IsUsernameUsedAsync(username))
            {
                throw new Exception("Username already used");
            }
            string hashedPassword = HashService.PasswordHasher(password);
            Utilisateur user = new Utilisateur
            {
                Username = username,
                FullName = fullName,
                Password = hashedPassword,
                Email = email
            };
            await _utilisateurRepository.AddUserAsync(user);
        }

        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email) ?? throw new Exception("User not found");
            user.Password = HashService.PasswordHasher(newPassword);
            await _utilisateurRepository.UpdateUserAsync(user);
            _passwordResetCodes.Remove(email);
        }

        public async Task<string> ForgottenPasswordAsync(string username)
        {
            var user = await _utilisateurRepository.GetByUsernameAsync(username) ?? throw new Exception("User not found");
            string code = new Random().Next(100000, 999999).ToString();
            _passwordResetCodes[user.Email] = (code, DateTime.Now.AddMinutes(5));

            await _emailService.SendPasswordResetCodeAsync(user.Email, code);
            return user.Email;
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email) ?? throw new Exception("User not found");
            string code = new Random().Next(100000, 999999).ToString();
            _passwordResetCodes[email] = (code, DateTime.Now.AddMinutes(5));
            await _emailService.SendVerificationCodeAsync(email, code);
        }

        public async Task<bool> IsResetCodeValidAsync(string email, string code)
        {
            if (_passwordResetCodes.TryGetValue(email, out var result))
            {
                if (result.Expiry > DateTime.Now && result.Code == code)
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> IsVerificationCodeValidAsync(string email, string code)
        {
            if (_passwordResetCodes.TryGetValue(email, out var result))
            {
                if (result.Expiry > DateTime.Now && result.Code == code)
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task VerifyUserAsync(string email)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email) ?? throw new Exception("User not found");
            user.UserVerified = true;
            await _utilisateurRepository.UpdateUserAsync(user);
        }
    }
}
