using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public enum CategoriaDesafio
{
    [Display(Name = "SQL Injection")]
    SqlInjection,

    [Display(Name = "Reflected XSS")]
    ReflectedXSS,

    [Display(Name = "Stored XSS")]
    StoredXSS,

    [Display(Name = "Broken Anti-Automation")]
    BrokenAntiAutomation,

    [Display(Name = "Security Misconfiguration")]
    SecurityMisconfiguration,

    [Display(Name = "Broken Authentication")]
    BrokenAuthentication,

    [Display(Name = "XXE")]
    XXE,

    [Display(Name = "Insecure Deserialization")]
    InsecureDeserialization,

    [Display(Name = "IDOR")]
    IDOR,

    [Display(Name = "Excessive Data Exposure")]
    ExcessiveDataExposure,

    [Display(Name = "Improper Input Validation")]
    ImproperInputValidation,

    [Display(Name = "Parameter Tampering")]
    ParameterTampering,
    
    [Display(Name = "Outros")]
    Outros
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        return attribute?.Name ?? value.ToString();
    }
}