using CineTrack.Data.Models;
using CineTrack.Data.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;    
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
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
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
            var utilisateur = new Utilisateur
            {
                Username = Username.Trim(),
                FullName = FullName.Trim(),
                Email = Email.Trim(),
                Password = Password

            };

            var context = new ValidationContext(utilisateur);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(utilisateur, context, results, validateAllProperties: true);

            if (isValid) {
                var proprietes = results[0].MemberNames.FirstOrDefault();
                erreur = proprietes switch
                {
                    nameof(Utilisateur.Username) => "Le nom d'utilisateur est invalide.",
                    nameof(Utilisateur.FullName) => "Le nom complet est requis.",
                    nameof(Utilisateur.Email) => "L'adresse courriel n'est pas valide.",
                    nameof(Utilisateur.Password) => "Le mot de passe doit contenir au moins 10 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial.",
                    _ => results[0].ErrorMessage ?? "Champ invalide."
                };
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
                _utilisateurService.SignUp(Username.Trim(), FullName.Trim(), Email.Trim(), Password);

                // TODO : naviguer vers SignIn après inscription réussie
                // _navigationService.NavigateTo<SignInViewModel>();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message switch
                {
                    "Email already used"    => "Cette adresse courriel est déjà utilisée.",
                    "Username already used" => "Ce nom d'utilisateur est déjà pris.",
                    _                       => "Une erreur est survenue. Veuillez réessayer."
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
