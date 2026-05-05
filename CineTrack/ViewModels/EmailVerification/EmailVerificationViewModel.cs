using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.Services.Interfaces;
using CineTrack.Session;
using CineTrack.ViewModels.Profile;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CineTrack.ViewModels.Auth.Verification
{
    public partial class EmailVerificationViewModel : ObservableObject, ITransferParameter
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly INavigationService _navigationService;

        [ObservableProperty] private string _verificationCode = string.Empty;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private string _successMessage = string.Empty;
        [ObservableProperty] private bool _isLoading = false;
        [ObservableProperty] private string _userEmail = string.Empty;

        public EmailVerificationViewModel(IUtilisateurService utilisateurService, INavigationService navigationService)
        {
            _utilisateurService = utilisateurService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task VerifyCodeAsync()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            try
            {
                bool isValid = await _utilisateurService.IsVerificationCodeValidAsync(UserEmail, VerificationCode);
                if (isValid)
                {
                    await _utilisateurService.VerifyUserAsync(UserEmail);
                    
                    SessionManager.Instance.CurrentUser.UserVerified = true;
                    SuccessMessage = "✅ Compte vérifié! Vous pouvez maintenant commenter.";
                }
                else
                {
                    ErrorMessage = "Code invalide ou expiré.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ResendCodeAsync()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;
            try
            {
                await _utilisateurService.SendVerificationCodeAsync(UserEmail);
                SuccessMessage = "New validation code sent";
            }
            catch (Exception ex) { ErrorMessage = ex.Message; }
            finally { IsLoading = false; }
        }

        [RelayCommand]
        private async Task GoBackAsync() => _navigationService.NavigateTo<ProfileViewModel>();

        public void TransferParameter(object param)
        {
            if (param is EmailVerificationParam emailParam)
            {
                UserEmail = emailParam.Email;

                
            }
        }
    }
}