using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.Interfaces;
using CineTrack.Services;
using CineTrack.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
namespace CineTrack.ViewModels.Auth
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
        private string _username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMsg = string.Empty;

        [ObservableProperty]
        private bool _isPasswordVisible = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
        private bool _isLoading = false;

        public SignInViewModel(IUtilisateurService utilisateurService, INavigationService navigationService) {
            _utilisateurService = utilisateurService;
            _navigationService = navigationService;
        }

        private bool CanSignIn() =>
            !String.IsNullOrWhiteSpace(Username) &&
            !String.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

        [RelayCommand(CanExecute = nameof(CanSignIn))]
        private async Task SignIn() {
            
            ErrorMsg = string.Empty;
            IsLoading = true;

            try
            {
               var user = await Task.Run(() =>
                    _utilisateurService.SignIn(Username.Trim(), Password)
                );
                SessionManager.Instance.OpenSession(user);

            }
            catch (Exception e)
            {
                ErrorMsg = e.Message switch
                {
                    "Invalid " => "Nom d'utilisateur ou mot de passe incorrect.",
                    _ => "Une erreur est survenue lors de la connexion."
                };
            }
            finally {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        private void NavigateToSignUp()
        {
             _navigationService.NavigateTo<SignUpViewModel>(); // corriger was singIn instead of singUp
        }
    }




}

