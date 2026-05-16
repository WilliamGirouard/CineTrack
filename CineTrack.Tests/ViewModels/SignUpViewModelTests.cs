using CineTrack.Data.Services.UtilisateurServ;
using Moq;
using CineTrack.Data.Services.NavigationServ;

namespace CineTrack.Tests.ViewModels
{
    public class SignUpViewModelTests
    {
        private readonly Mock<IUtilisateurService> _mockService;
        private readonly Mock<INavigationService> _mockNav;
    }
}
