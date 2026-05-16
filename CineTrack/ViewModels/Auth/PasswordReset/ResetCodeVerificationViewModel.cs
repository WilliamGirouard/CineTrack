using CineTrack.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.Data.Services.UtilisateurServ;

namespace CineTrack.ViewModels.Auth.PasswordReset
{
    public partial class ResetCodeVerificationViewModel : ObservableObject, ITransferParameter
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly INavigationService _navigationService;
        
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResetCodeVerificationCommand))]
        private string _code = string.Empty;


        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResetCodeVerificationCommand))]
        private bool _isLoading = false;

        public ResetCodeVerificationViewModel(IUtilisateurService utilisateurService, INavigationService navigationService)
        {
            _utilisateurService = utilisateurService;
            _navigationService = navigationService;
        }

        public void TransferParameter(object param)
        {
            if (param is string email)
            {
                _email = email;
            }
        }

        private bool CanResetCodeVerification() =>
            !string.IsNullOrWhiteSpace(Code) &&
            !IsLoading;

        [RelayCommand(CanExecute = nameof(CanResetCodeVerification))]
        private async Task ResetCodeVerification()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;
            try
            {
                bool isResetCodeValid = await _utilisateurService.IsResetCodeValidAsync(_email, Code.Trim());
                if (!isResetCodeValid)
                {
                    ErrorMessage = "Code invalide/expiré";
                    return;
                }
                _navigationService.NavigateTo<ResetPasswordViewModel>(_email);
            }
            catch (Exception e)
            {
                ErrorMessage = $"Il y a une erreur : {e.Message}";
            }
            finally
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
