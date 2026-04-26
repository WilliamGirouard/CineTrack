using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.Services.Interfaces;
using CineTrack.Session;
using CineTrack.ViewModels.Auth.PasswordReset;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private bool _rememberMe = false;

        public SignInViewModel(IUtilisateurService utilisateurService, INavigationService navigationService)
        {
            _utilisateurService = utilisateurService;
            _navigationService = navigationService;

            // Restaurer la session si Remember Me est coché
            if (Properties.Settings.Default.RememberMe)
            {
                Username = Properties.Settings.Default.SavedUsername;
                Password = Properties.Settings.Default.SavedPassword;  
                RememberMe = true;
            }

        }

        private bool CanSignIn() =>
            !String.IsNullOrWhiteSpace(Username) &&
            !String.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

        [RelayCommand(CanExecute = nameof(CanSignIn))]
        private async Task SignIn()
        {

            ErrorMsg = string.Empty;
            IsLoading = true;

            try
            {
                var user = await _utilisateurService.SignInAsync(Username.Trim(), Password);
                SessionManager.Instance.OpenSession(user);


                //en soit sa vas nous permettre de garder la session ouverte dans notre porpre disque
                if (RememberMe)
                {
                    Properties.Settings.Default.RememberMe = true;
                    Properties.Settings.Default.SavedUsername = Username;
                    Properties.Settings.Default.SavedPassword = Password;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Reset();
                }

                _navigationService.NavigateTo<MainViewModel>();

            }
            catch (Exception e)
            {
                ErrorMsg = e.Message switch
                {
                    "Invalid credentials" => "Nom d'utilisateur ou mot de passe incorrect.",
                    _ => "Une erreur est survenue lors de la connexion."
                };
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
        private void NavigateToSignUp()
        {
            _navigationService.NavigateTo<SignUpViewModel>();
        }

        [RelayCommand]
        private void NavigateToForgottenPassword()
        {
            _navigationService.NavigateTo<ForgottenPasswordViewModel>();
        }
    }
}
