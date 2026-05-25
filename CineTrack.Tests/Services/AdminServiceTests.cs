using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.AdminServ;
using Moq;

namespace CineTrack.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IUtilisateurRepository> _mockUserRepo;
        private readonly Mock<ICommentaireRepository> _mockCommentaireRepo;
        private readonly Mock<INoteRepository> _mockNoteRepo;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _mockUserRepo = new Mock<IUtilisateurRepository>();
            _mockCommentaireRepo = new Mock<ICommentaireRepository>();
            _mockNoteRepo = new Mock<INoteRepository>();
            _service = new AdminService(_mockUserRepo.Object, _mockCommentaireRepo.Object, _mockNoteRepo.Object);
        }

        // ElevateUserToAdmin => devient admin, exception si déjà admin ou absent
        [Fact]
        public async Task ElevateUserToAdminAsync_UtilisateurUser_DevientAdmin()
        {
            var user = new Utilisateur { Id = 1, Role = EnumRole.user };
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(user);

            await _service.ElevateUserToAdminAsync(1);

            _mockUserRepo.Verify(r => r.UpdateUserAsync(It.Is<Utilisateur>(u => u.Role == EnumRole.admin)), Times.Once);
        }

        [Fact]
        public async Task ElevateUserToAdminAsync_DejaAdminOuAbsent_LanceException()
        {
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.admin });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(666)).ReturnsAsync((Utilisateur?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.ElevateUserToAdminAsync(1));
            await Assert.ThrowsAsync<Exception>(() => _service.ElevateUserToAdminAsync(666));
        }

        // DeleteUser => supprimé si user, exception si admin ou absent
        [Fact]
        public async Task DeleteUserAsync_UtilisateurUser_EstSupprime()
        {
            var user = new Utilisateur { Id = 1, Role = EnumRole.user };
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(user);

            await _service.DeleteUserAsync(1);

            _mockUserRepo.Verify(r => r.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_AdminOuAbsent_LanceException()
        {
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.admin });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(666)).ReturnsAsync((Utilisateur?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteUserAsync(1));
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteUserAsync(666));
        }

        // IsAdmin => true si admin, false si user, exception si absent
        [Fact]
        public async Task IsAdminAsync_AdminOuUser_RetourneBonResultat()
        {
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.admin });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(2)).ReturnsAsync(new Utilisateur { Id = 2, Role = EnumRole.user });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(666)).ReturnsAsync((Utilisateur?)null);

            Assert.True(await _service.IsAdminAsync(1));
            Assert.False(await _service.IsAdminAsync(2));
            await Assert.ThrowsAsync<Exception>(() => _service.IsAdminAsync(666));
        }

        // DeleteCommentaire => supprimé si user, exception si admin ou absent
        [Fact]
        public async Task DeleteCommentaireAsync_CommentaireUser_EstSupprime()
        {
            _mockCommentaireRepo.Setup(r => r.GetCommentaireAsync(1)).ReturnsAsync(new Commentaire { Id = 1, UtilisateurId = 1 });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.user });

            await _service.DeleteCommentaireAsync(1);

            _mockCommentaireRepo.Verify(r => r.RemoveCommentaireAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCommentaireAsync_AdminOuAbsent_LanceException()
        {
            _mockCommentaireRepo.Setup(r => r.GetCommentaireAsync(1)).ReturnsAsync(new Commentaire { Id = 1, UtilisateurId = 1 });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.admin });
            _mockCommentaireRepo.Setup(r => r.GetCommentaireAsync(666)).ReturnsAsync((Commentaire?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteCommentaireAsync(1));
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteCommentaireAsync(666));
        }

        // DeleteNote => supprimée si user, exception si admin ou absente
        [Fact]
        public async Task DeleteNoteAsync_NoteUser_EstSupprimee()
        {
            _mockNoteRepo.Setup(r => r.GetNoteByIdAsync(1)).ReturnsAsync(new Note { Id = 1, UtilisateurId = 1 });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.user });

            await _service.DeleteNoteAsync(1);

            _mockNoteRepo.Verify(r => r.DeleteNoteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteNoteAsync_AdminOuAbsente_LanceException()
        {
            _mockNoteRepo.Setup(r => r.GetNoteByIdAsync(1)).ReturnsAsync(new Note { Id = 1, UtilisateurId = 1 });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.admin });
            _mockNoteRepo.Setup(r => r.GetNoteByIdAsync(666)).ReturnsAsync((Note?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteNoteAsync(1));
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteNoteAsync(666));
        }

        // DeleteAllCommentaires => supprime les users, ignore les admins
        [Fact]
        public async Task DeleteAllCommentairesAsync_IgnoreAdmins_SupprimeUsers()
        {
            var commentaires = new List<Commentaire>
            {
                new Commentaire { Id = 1, UtilisateurId = 1 },
                new Commentaire { Id = 2, UtilisateurId = 2 }
            };
            _mockCommentaireRepo.Setup(r => r.GetAllCommentairesAsync()).ReturnsAsync(commentaires);
            _mockCommentaireRepo.Setup(r => r.GetCommentaireAsync(1)).ReturnsAsync(new Commentaire { Id = 1, UtilisateurId = 1 });
            _mockCommentaireRepo.Setup(r => r.GetCommentaireAsync(2)).ReturnsAsync(new Commentaire { Id = 2, UtilisateurId = 2 });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(1)).ReturnsAsync(new Utilisateur { Id = 1, Role = EnumRole.user });
            _mockUserRepo.Setup(r => r.GetUtilisateurByIdAsync(2)).ReturnsAsync(new Utilisateur { Id = 2, Role = EnumRole.admin });

            await _service.DeleteAllCommentairesAsync();

            _mockCommentaireRepo.Verify(r => r.RemoveCommentaireAsync(1), Times.Once);
            _mockCommentaireRepo.Verify(r => r.RemoveCommentaireAsync(2), Times.Never);
        }
    }
}