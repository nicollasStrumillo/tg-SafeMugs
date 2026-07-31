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

    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        return attribute?.Description ?? value.ToString();
    }

    public static bool TryGetEnumByNomeDisplay<T>(string nomeDisplay, out T enumValue)
    where T : struct, Enum
    {
        foreach (var value in Enum.GetValues<T>())
        {
            if (string.Equals(value.GetNomeDisplay(), nomeDisplay, StringComparison.Ordinal))
            {
                enumValue = value;
                return true;
            }
        }
        enumValue = default;
        return false;
    }
}
