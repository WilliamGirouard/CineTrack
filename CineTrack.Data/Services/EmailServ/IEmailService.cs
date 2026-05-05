namespace CineTrack.Data.Services.EmailServ
{
    public interface IEmailService
    {
        Task SendPasswordResetCodeAsync(string userEmail, string code);
        Task SendVerificationCodeAsync(string userEmail, string code);
    }
}
