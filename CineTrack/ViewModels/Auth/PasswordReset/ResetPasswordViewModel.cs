using CineTrack.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Services.UtilisateurServ;

namespace CineTrack.ViewModels.Auth.PasswordReset
{
    public partial class ResetPasswordViewModel : ObservableObject, ITransferParameter
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly INavigationService _navigationService;

        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResetPasswordCommand))]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResetPasswordCommand))]
        private string _confirmPassword = string.Empty;


        [ObservableProperty]
        private bool _isPasswordVisible = false;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResetPasswordCommand))]
        private bool _isLoading = false;

        public ResetPasswordViewModel(IUtilisateurService utilisateurService, INavigationService navigationService)
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
        private bool CanResetPassword() =>
            !string.IsNullOrWhiteSpace(NewPassword) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            NewPassword == ConfirmPassword &&
            !IsLoading;

        [RelayCommand(CanExecute = nameof(CanResetPassword))]
        private async Task ResetPassword()
        {
            ErrorMessage = string.Empty;
            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Les mots de passes ne sont pas les mêmes";
                return;
            }
            IsLoading = true;
            try
            {
                await _utilisateurService.ResetPasswordAsync(_email, NewPassword);
                _navigationService.NavigateTo<SignInViewModel>();
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
        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        private void NavigateToSignIn()
        {
            _navigationService.NavigateTo<SignInViewModel>();
        }

    }
}
