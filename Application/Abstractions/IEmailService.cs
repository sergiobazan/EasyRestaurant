namespace Application.Abstractions;

public interface IEmailService
{
    Task SendEmail(string to, string subject, string emailBody);
}
