using CineTrack.Data.Models;
using CineTrack.Data.Services.EmailServ;
using CineTrack.Data.Services.PasswordResetStoreServ;
using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.Tests.Fakes;
using Moq;

namespace CineTrack.Tests.Services
{
    public class UtilisateurServiceTests
    {
        private readonly FakeUtilisateurRepository _fakeRepo;
        private readonly Mock<IEmailService>       _mockEmailService;
        private readonly PasswordResetStore        _passwordResetStore;
        private readonly UtilisateurService        _service;

        public UtilisateurServiceTests()
        {
            _fakeRepo           = new FakeUtilisateurRepository();
            _mockEmailService   = new Mock<IEmailService>();
            _passwordResetStore = new PasswordResetStore();
            _service            = new UtilisateurService(_fakeRepo, _mockEmailService.Object, _passwordResetStore);
        }

        // SignUp avec un nouvel utilisateur => ajouté avec mot de passe haché
        [Fact]
        public async Task SignUpAsync_NouvelUtilisateur_EstAjouteAvecMotDePasseHache()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            Assert.Single(_fakeRepo.Utilisateurs);
            Assert.NotEqual("Password123!", _fakeRepo.Utilisateurs[0].Password);
        }

        // SignUp avec email ou username déjà utilisé => exception
        [Fact]
        public async Task SignUpAsync_EmailOuUsernameDejaUtilise_LanceException()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            await Assert.ThrowsAsync<Exception>(() =>
                _service.SignUpAsync("AutreUsername", "Autre", "jackiechan67@gmail.com", "Password456!"));
            await Assert.ThrowsAsync<Exception>(() =>
                _service.SignUpAsync("JackieChan67", "Autre", "autre@gmail.com", "Password456!"));
        }

        // SignIn avec bons credentials => retourne l'utilisateur
        [Fact]
        public async Task SignInAsync_CredentialsValides_RetourneUtilisateur()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            var result = await _service.SignInAsync("JackieChan67", "Password123!");

            Assert.NotNull(result);
            Assert.Equal("JackieChan67", result.Username);
        }

        // SignIn avec mauvais credentials => exception
        [Fact]
        public async Task SignInAsync_MauvaisCredentials_LanceException()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            await Assert.ThrowsAsync<Exception>(() => _service.SignInAsync("JackieChan67", "MauvaisMotDePasse!"));
            await Assert.ThrowsAsync<Exception>(() => _service.SignInAsync("UtilisateurFantome", "Password123!"));
        }

        // IsUsernameUsedAsync / IsEmailUsedAsync => true si présent, false sinon
        [Fact]
        public async Task IsUsernameOrEmailUsedAsync_PresentOuAbsent_RetourneBonResultat()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            Assert.True(await _service.IsUsernameUsedAsync("JackieChan67"));
            Assert.False(await _service.IsUsernameUsedAsync("Fantome"));
            Assert.True(await _service.IsEmailUsedAsync("jackiechan67@gmail.com"));
            Assert.False(await _service.IsEmailUsedAsync("fantome@gmail.com"));
        }

        // ForgottenPassword => retourne l'email et envoie le code
        [Fact]
        public async Task ForgottenPasswordAsync_UtilisateurPresent_RetourneEmailEtEnvoieCode()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            var email = await _service.ForgottenPasswordAsync("JackieChan67");

            Assert.Equal("jackiechan67@gmail.com", email);
            _mockEmailService.Verify(e => e.SendPasswordResetCodeAsync("jackiechan67@gmail.com", It.IsAny<string>()), Times.Once);
        }

        // IsResetCodeValidAsync => true si code valide, false si expiré ou mauvais
        [Fact]
        public async Task IsResetCodeValidAsync_CodeValideOuInvalide_RetourneBonResultat()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");
            await _service.ForgottenPasswordAsync("JackieChan67");
            var code = _passwordResetStore.Codes["jackiechan67@gmail.com"].Code;

            Assert.True(await _service.IsResetCodeValidAsync("jackiechan67@gmail.com", code));
            Assert.False(await _service.IsResetCodeValidAsync("jackiechan67@gmail.com", "000000"));
        }

        // ResetPassword => nouveau mot de passe fonctionne, code supprimé
        [Fact]
        public async Task ResetPasswordAsync_UtilisateurPresent_MotDePasseMisAJourEtCodeSupprime()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");
            await _service.ForgottenPasswordAsync("JackieChan67");

            await _service.ResetPasswordAsync("jackiechan67@gmail.com", "NouveauMotDePasse123!");

            Assert.NotNull(await _service.SignInAsync("JackieChan67", "NouveauMotDePasse123!"));
            Assert.False(_passwordResetStore.Codes.ContainsKey("jackiechan67@gmail.com"));
        }

        // SendVerificationCode => envoie l'email avec le code
        [Fact]
        public async Task SendVerificationCodeAsync_UtilisateurPresent_EnvoieEmail()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            await _service.SendVerificationCodeAsync("jackiechan67@gmail.com");

            _mockEmailService.Verify(e => e.SendVerificationCodeAsync("jackiechan67@gmail.com", It.IsAny<string>()), Times.Once);
        }

        // IsVerificationCodeValidAsync => true si code valide, false si expiré ou mauvais
        [Fact]
        public async Task IsVerificationCodeValidAsync_CodeValideOuInvalide_RetourneBonResultat()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");
            await _service.SendVerificationCodeAsync("jackiechan67@gmail.com");
            var code = _passwordResetStore.Codes["jackiechan67@gmail.com"].Code;

            Assert.True(await _service.IsVerificationCodeValidAsync("jackiechan67@gmail.com", code));
            Assert.False(await _service.IsVerificationCodeValidAsync("jackiechan67@gmail.com", "000000"));
        }

        // VerifyUser => UserVerified passe à true
        [Fact]
        public async Task VerifyUserAsync_UtilisateurPresent_UserVerifiedEstTrue()
        {
            await _service.SignUpAsync("JackieChan67", "Jackie Chan", "jackiechan67@gmail.com", "Password123!");

            await _service.VerifyUserAsync("jackiechan67@gmail.com");

            Assert.True(_fakeRepo.Utilisateurs[0].UserVerified);
        }
    }
}