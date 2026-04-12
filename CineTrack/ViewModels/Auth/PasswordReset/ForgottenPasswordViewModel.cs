using CineTrack.Data.Services.Interfaces;
using CineTrack.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace CineTrack.ViewModels.Auth.PasswordReset
{
    public partial class ForgottenPasswordViewModel : ObservableObject
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendPasswordResetCodeCommand))]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendPasswordResetCodeCommand))]
        private bool _isLoading = false;

        public ForgottenPasswordViewModel(IUtilisateurService utilisateurService, INavigationService navigationService)
        {
            _utilisateurService = utilisateurService;
            _navigationService = navigationService;
        }
        private bool CanSendPasswordResetCode() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !IsLoading;
        
        [RelayCommand(CanExecute = nameof(CanSendPasswordResetCode))]
        private async Task SendPasswordResetCode()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;
            try 
            {
                var email = await _utilisateurService.ForgottenPasswordAsync(Username.Trim());
                _navigationService.NavigateTo<ResetCodeVerificationViewModel>(email);
            } catch (Exception e)
            {
                ErrorMessage = e.Message switch
                {
                    "User not found" => "Nom d'utilisateur invalide",
                    _ => "Erreur"
                };
            } finally 
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private void NavigateToSignIn()
        {
            _navigationService.NavigateTo<SignInViewModel>();
        }
    }
}
