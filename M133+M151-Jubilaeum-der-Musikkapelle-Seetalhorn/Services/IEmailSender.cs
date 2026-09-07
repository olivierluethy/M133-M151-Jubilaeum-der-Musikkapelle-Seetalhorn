namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}
