using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.Session;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Auth.PasswordReset;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CineTrack.ViewModels.Settings
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IUtilisateurService _utilisateurService;
        private readonly IUtilisateurRepository _utilisateurRepository;

        // Profile 
        public string DisplayFullName => SessionManager.Instance.CurrentUser?.FullName ?? "";
        public string DisplayUsername => SessionManager.Instance.CurrentUser?.Username ?? "";
        public string DisplayEmail => SessionManager.Instance.CurrentUser?.Email ?? "";

        // Email
        [ObservableProperty] private string newEmail = string.Empty;
        [ObservableProperty] private string emailMessage = string.Empty;
        [ObservableProperty] private bool emailSuccess;

        // Username
        [ObservableProperty] private string newUsername = string.Empty;
        [ObservableProperty] private string usernameMessage = string.Empty;
        [ObservableProperty] private bool usernameSuccess;

        public SettingsViewModel(
            INavigationService navigationService,
            IUtilisateurService utilisateurService,
            IUtilisateurRepository utilisateurRepository)
        {
            _navigationService = navigationService;
            _utilisateurService = utilisateurService;
            _utilisateurRepository = utilisateurRepository;
        }

        [RelayCommand]
        private void GoBack() => _navigationService.NavigateTo<MainViewModel>();

        [RelayCommand]
        private void ChangePassword()
        {
            var email = SessionManager.Instance.CurrentUser?.Email;
            if (email == null) return;
            SessionManager.Instance.CloseSession();
            _navigationService.NavigateTo<ResetPasswordViewModel>(email);
        }

        [RelayCommand]
        private async Task ChangeEmailAsync()
        {
            EmailMessage = string.Empty;
            EmailSuccess = false;

            var user = SessionManager.Instance.CurrentUser;
            if (user == null) return;

            if (string.IsNullOrWhiteSpace(NewEmail))
            {
                EmailMessage = "Veuillez saisir une adresse e-mail.";
                return;
            }

            if (NewEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                EmailMessage = "C'est déjà votre adresse e-mail.";
                return;
            }

            if (await _utilisateurService.IsEmailUsedAsync(NewEmail))
            {
                EmailMessage = "Cette adresse e-mail est déjà utilisée.";
                return;
            }

            user.Email = NewEmail;
            await _utilisateurRepository.UpdateUserAsync(user);
            NewEmail = string.Empty;
            EmailSuccess = true;
            EmailMessage = "Adresse e-mail modifiée avec succès.";
            OnPropertyChanged(nameof(DisplayEmail));
        }

        [RelayCommand]
        private async Task ChangeUsernameAsync()
        {
            UsernameMessage = string.Empty;
            UsernameSuccess = false;

            var user = SessionManager.Instance.CurrentUser;
            if (user == null) return;

            if (string.IsNullOrWhiteSpace(NewUsername))
            {
                UsernameMessage = "Veuillez saisir un nom d'utilisateur.";
                return;
            }

            if (NewUsername.Equals(user.Username, StringComparison.OrdinalIgnoreCase))
            {
                UsernameMessage = "C'est déjà votre nom d'utilisateur.";
                return;
            }

            if (await _utilisateurService.IsUsernameUsedAsync(NewUsername))
            {
                UsernameMessage = "Ce nom d'utilisateur est déjà pris.";
                return;
            }

            user.Username = NewUsername;
            await _utilisateurRepository.UpdateUserAsync(user);
            NewUsername = string.Empty;
            UsernameSuccess = true;
            UsernameMessage = "Nom d'utilisateur modifié avec succès.";
            OnPropertyChanged(nameof(DisplayUsername));
        }

        // JSP si on garde lui en vrai mais toujours bon à avoir just in case ngl
        [RelayCommand]
        private async Task DeleteAccountAsync()
        {
            var user = SessionManager.Instance.CurrentUser;
            if (user == null) return;

            await _utilisateurRepository.DeleteUserAsync(user);
            SessionManager.Instance.CloseSession();
            _navigationService.NavigateTo<SignInViewModel>();
        }
    }
}