using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Project.SaaS.Certfy.Core.Extensions;

public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Type, string[]> _cache = new();

    public static string? GetDisplayName(this Enum @enum)
    {
        if (@enum == null) return null;

        var member = @enum.GetType()
                          .GetMember(@enum.ToString())
                          .FirstOrDefault();

        if (member == null) return @enum.ToString();

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? @enum.ToString();
    }

    public static string[] GetDisplayNames<TEnum>() where TEnum : Enum
    {
        return typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(f =>
            {
                var display = f.GetCustomAttribute<DisplayAttribute>();
                return display?.Name ?? f.Name;
            })
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .ToArray();
    }
}
