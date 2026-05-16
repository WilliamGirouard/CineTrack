using CineTrack.Data.Services.UtilisateurServ;
using Moq;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.ViewModels.Auth;

namespace CineTrack.Tests.ViewModels
{
    public class SignUpViewModelTests
    {
        private readonly Mock<IUtilisateurService> _mockService;
        private readonly Mock<INavigationService> _mockNav;
        private readonly SignUpViewModel _vm;

        public SignUpViewModelTests()
        {
            _mockService = new Mock<IUtilisateurService>();
            _mockNav = new Mock<INavigationService>();

            _mockService
                .Setup(s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockService
                .Setup(s => s.SendVerificationCodeAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _vm = new SignUpViewModel(_mockService.Object, _mockNav.Object);
        }

        private void RemplirChampsValides()
        {
            _vm.FullName = "Jackie Chan";
            _vm.Username = "JackieChant67";
            _vm.Email = "jackiechan67@gmail.com";
            _vm.Password = "JackieChan67$$JackieChan67$$";
            _vm.ConfirmPassword = "JackieChan67$$JackieChan67$$";
            _vm.IsTermsAccepted = true;
        }

        [Fact]
        public void CanSignUp_ChampsVides_RetourneFalse()
        {
            Assert.False(_vm.SignUpCommand.CanExecute(null));
        }

        [Fact]
        public void CanSignUp_TermesEtConditionsNonAcceptes_RetourneFalse()
        {
            RemplirChampsValides();
            _vm.IsTermsAccepted = false;

            Assert.False(_vm.SignUpCommand.CanExecute(null));
        }

        [Fact]
        public void CanSignUp_ChampsValides_RetourneTrue()
        {
            RemplirChampsValides();
            Assert.True(_vm.SignUpCommand.CanExecute(null));
        }



        [Fact]
        public async Task SignUp_MotsDePassesPasIdentiques_ErreurAffiche()
        {
            RemplirChampsValides();
            _vm.ConfirmPassword = "PROUTproutProut696969$$$$$$!!!!";

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.Contains("ne correspondent pas", _vm.ErrorMessage);
            _mockService.Verify(
                s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task SignUp_EmailPasValide_ErreurAfficheEmail()
        {
            RemplirChampsValides();
            _vm.Email = "prout";

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.Contains("courriel", _vm.ErrorMessage);
            _mockService.Verify(
                s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task SignUp_PasswordNonConforme_ErreurAfficheMotDePasse()
        {
            RemplirChampsValides();
            _vm.Password = "password";
            _vm.ConfirmPassword = "password";

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.Contains("Le mot de passe doit contenir au moins 10", _vm.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SignUp_ChampsValides_AppelleSignUp()
        {
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            _mockService.Verify(
                s => s.SignUpAsync("JackieChant67", "Jackie Chan", "jackiechan67@gmail.com", "JackieChan67$$JackieChan67$$"),
                Times.Once);
        }

        [Fact]
        public async Task SignUp_ChampsValides_AppelleVerifCode()
        {
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            _mockService.Verify(
                s => s.SendVerificationCodeAsync("jackiechan67@gmail.com"),
                Times.Once);
        }

        [Fact]
        public async Task SignUp_ChampsValides_NavigationToSignIn()
        {
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            _mockNav.Verify(
                n => n.NavigateTo<SignInViewModel>(),
                Times.Once);
        }

        [Fact]
        public async Task SignUp_ChampsValides_IsLoadingFalseApresSignUpValide()
        {
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.False(_vm.IsLoading);
        }


        [Theory]
        [InlineData("Email already used", "Cette adresse courriel est déjà utilisée.")]
        [InlineData("Username already used", "Ce nom d'utilisateur est déjà pris.")]
        [InlineData("Error", "Une erreur est survenue. Veuillez réessayer")]
        public async Task SignUp_ExceptionLorsSignUp_ErreurAfficheFR(string messageException, string messagePrevu)
        {
            _mockService
                .Setup(s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(messageException));

            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.Contains(messagePrevu, _vm.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SignUp_EmailDejaPris_AppellePasNavigateTo()
        {
            _mockService
                .Setup(s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Email already used"));
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            _mockNav.Verify(
                n => n.NavigateTo<SignInViewModel>(),
                Times.Never);
        }

        [Fact]
        public async Task SignUp_ExceptionPendantSignUpAsync_IsLoadingAlwaysReset()
        {
            _mockService
                .Setup(s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erreur"));
            RemplirChampsValides();

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.False(_vm.IsLoading);
        }

        [Fact]
        public async Task SignUp_ChampsAvecEspaces_TrimLesChamps()
        {
            string? usernameRecupere = null;
            string? emailRecupere = null;
            string? fullNameRecupere = null;

            _mockService
                .Setup(s => s.SignUpAsync(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>(
                    (username, fullName, email, _) =>
                    {
                        usernameRecupere = username;
                        emailRecupere = email;
                        fullNameRecupere = fullName;
                    })
                .Returns(Task.CompletedTask);

            RemplirChampsValides();
            _vm.Username = " JackieChan67   ";
            _vm.Email = "  jackChain67@gmail.com    ";
            _vm.FullName = "      Jackie Chan      ";

            await _vm.SignUpCommand.ExecuteAsync(null);

            Assert.Equal("JackieChan67", usernameRecupere);
            Assert.Equal("jackChain67@gmail.com", emailRecupere);
            Assert.Equal("Jackie Chan", fullNameRecupere);
        }
    }
}
