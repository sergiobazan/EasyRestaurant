using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Services;

internal class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmail(string to, string subject, string emailBody)
    {
        _logger.LogInformation("Process email to {to} with subject {subject} and body {emailBody}", to, subject, emailBody);

        return Task.CompletedTask;
    }
}
