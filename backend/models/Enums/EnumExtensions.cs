using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public static class EnumExtensions
{
    public static string GetNomeDisplay(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        return attribute?.Name ?? value.ToString();
    }

    public static string GetHashSenha(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        return attribute?.Description ?? value.ToString();
    }
}
