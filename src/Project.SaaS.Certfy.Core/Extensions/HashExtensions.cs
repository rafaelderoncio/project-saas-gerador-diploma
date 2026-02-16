using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Project.SaaS.Certfy.Core.Extensions;

public static class HashExtensions
{
    public static string ToHash<T>(this T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        
        var json = JsonSerializer.Serialize(obj, options);

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(json);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}