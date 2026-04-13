
namespace CineTrack.Data.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetCodeAsync(string userEmail, string code);
    }
}
