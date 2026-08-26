namespace backend.SsrfConfiguration;

public class SsrfSettings
{
    public const string SectionName = "Ssrf";

    public string TargetScheme { get; set; } = "http";
    public List<string> TargetHost { get; set; } = ["127.0.0.1", "localhost"];
    public int TargetPort { get; set; } = 8080;
    public string TargetPath { get; set; } = "/api/usuario/requisicao-interna";
    public string SecretHeaderName { get; set; } = "X-Internal-Request";
    public string SecretHeaderValue { get; set; } = string.Empty;
}