using System.Net;
using System.Net.Mail;
using System.Text;
using backend.EmailConfiguration;
using backend.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace backend.Services.Implementations.Email;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public SmtpEmailSender(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task EnviarAsync(string destinatario, string assunto, string corpoHtml)
    {
        using var mensagem = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress),
            Subject = assunto,
            SubjectEncoding = Encoding.UTF8,
            Body = corpoHtml,
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = true
        };
        mensagem.To.Add(destinatario);

        using var cliente = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.EnableSsl,
            UseDefaultCredentials = false
        };

        if (!string.IsNullOrWhiteSpace(_settings.Username))
            cliente.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

        await cliente.SendMailAsync(mensagem);
    }
}
