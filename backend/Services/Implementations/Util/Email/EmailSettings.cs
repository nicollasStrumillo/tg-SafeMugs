namespace backend.EmailConfiguration;

public sealed class EmailSettings
{
    public const string SectionName = "Email";

    public string SmtpHost { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string FromAddress { get; set; } = "email@gmail.com";
    public string FrontendBaseUrl { get; set; } = "http://localhost:4200";
    public int ResetTokenLifetimeMinutes { get; set; } = 15;
}
