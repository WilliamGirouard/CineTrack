using CineTrack.Data.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;

namespace CineTrack.ViewModels.Auth
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IUtilisateurService _utilisateurService;



        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _fullName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _confirmPassword = string.Empty;


        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isPasswordVisible = false;

        [ObservableProperty]
        private bool _isLoading = false;


        public SignUpViewModel(IUtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
        }


        private bool CanSignUp() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            !IsLoading;

        private bool ValiderChamps(out string erreur)
        {
            if (Username.Length < 3)
            {
                erreur = "Le nom d'utilisateur doit contenir au moins 3 caractères.";
                return false;
            }
            if (FullName.Trim().Length < 2)
            {
                erreur = "Le nom complet est requis.";
                return false;
            }
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                erreur = "L'adresse courriel n'est pas valide.";
                return false;
            }
            if (Password.Length < 8)
            {
                erreur = "Le mot de passe doit contenir au moins 8 caractères.";
                return false;
            }
            if (!Regex.IsMatch(Password, @"\d"))
            {
                erreur = "Le mot de passe doit contenir au moins un chiffre.";
                return false;
            }
            if (!Regex.IsMatch(Password, @"[!@#$%^&*(),.?""{|}|<>]"))
            {
                erreur = "Le mot de passe doit contenir au moins un caractère spécial.";
                return false;
            }
            if (Password != ConfirmPassword)
            {
                erreur = "Les mots de passe ne correspondent pas.";
                return false;
            }

            erreur = string.Empty;
            return true;
        }


        [RelayCommand(CanExecute = nameof(CanSignUp))]
        private async Task SignUp()
        {
            ErrorMessage = string.Empty;

            if (!ValiderChamps(out string erreur))
            {
                ErrorMessage = erreur;
                return;
            }

            IsLoading = true;
            SignUpCommand.NotifyCanExecuteChanged();

            try
            {
                await Task.Run(() =>
                    _utilisateurService.SignUp(Username.Trim(), FullName.Trim(), Email.Trim(), Password)
                );
                // TODO : naviguer vers SignIn après inscription réussie
                // _navigationService.NavigateTo<SignInViewModel>();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message switch
                {
                    "Email already used" => "Cette adresse courriel est déjà utilisée.",
                    "Username already used" => "Ce nom d'utilisateur est déjà pris.",
                    _ => "Une erreur est survenue. Veuillez réessayer."
                };
            }
            finally
            {
                IsLoading = false;
                SignUpCommand.NotifyCanExecuteChanged();
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
            // TODO : _navigationService.NavigateTo<SignInViewModel>();
        }
    }
}