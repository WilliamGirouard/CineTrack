using CineTrack.Data.Services.NavigationServ;
using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.ViewModels;
using CineTrack.ViewModels.Auth;
using Moq;

namespace CineTrack.Tests.ViewModels
{
    public class SignInViewModelTests
    {
        private readonly Mock<IUtilisateurService> _mockService;
        private readonly Mock<INavigationService> _mockNav;
        private readonly SignInViewModel _vm;

        public SignInViewModelTests()
        {
            _mockService = new Mock<IUtilisateurService>();
            _mockNav = new Mock<INavigationService>();
            _vm = new SignInViewModel(_mockService.Object, _mockNav.Object);
        }

        // Test 1: Le bouton SignIn est désactivé quand les champs sont vides
        [Fact]
        public void CanSignIn_ChampsVides_RetourneFalse()
        {
            Assert.False(_vm.SignInCommand.CanExecute(null));
        }

        // Test 2: Le bouton SignIn est actif quand les deux champs sont remplis
        [Fact]
        public void CanSignIn_ChampsRemplis_RetourneTrue()
        {
            _vm.Username = "JackieChan67";
            _vm.Password = "MotDePasse123!";

            Assert.True(_vm.SignInCommand.CanExecute(null));
        }

        // Test 3: Si le service lance "Invalid credentials", le message français s'affiche
        [Fact]
        public async Task SignIn_InvalidCredentials_AfficheErreurFrancais()
        {
            _mockService
                .Setup(s => s.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Invalid credentials"));

            _vm.Username = "mauvaisUser";
            _vm.Password = "mauvaisPass";

            await _vm.SignInCommand.ExecuteAsync(null);

            Assert.Contains("incorrect", _vm.ErrorMsg, StringComparison.OrdinalIgnoreCase);
        }

        // Test 4: IsLoading revient à false après une erreur (finally block)
        [Fact]
        public async Task SignIn_ExceptionLancee_IsLoadingToujoursReinitialisee()
        {
            _mockService
                .Setup(s => s.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Quelque erreur"));

            _vm.Username = "user";
            _vm.Password = "pass";

            await _vm.SignInCommand.ExecuteAsync(null);

            // Le block finally du ViewModel doit toujours remettre IsLoading à false
            Assert.False(_vm.IsLoading);
        }

        // Test 5: Si SignIn réussit, NavigateTo<MainViewModel> est appelé
        [Fact]
        public async Task SignIn_SuccesConnexion_NavigueVersMain()
        {
            var fakeUser = new CineTrack.Data.Models.Utilisateur
            {
                Id = 1,
                Username = "JackieChan67",
                FullName = "Jackie Chan",
                Email = "jackie@test.com",
                Password = "hashed"
            };

            _mockService
                .Setup(s => s.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(fakeUser);

            _vm.Username = "JackieChan67";
            _vm.Password = "MotDePasse123!";

            await _vm.SignInCommand.ExecuteAsync(null);

            _mockNav.Verify(n => n.NavigateTo<MainViewModel>(), Times.Once);
        }
    }
}

