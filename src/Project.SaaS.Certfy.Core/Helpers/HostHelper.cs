using Microsoft.AspNetCore.Http;

namespace Project.SaaS.Certfy.Core.Helpers;

public static class HostHelper
{
   public static string GetBaseUrl(HttpContext context)
    {
        if (context != null)
        {
            var request = context.Request;

            var scheme = request.Scheme;
            var host = request.Host.Value;

            return $"{scheme}://{host}";
        }

        throw new ArgumentNullException(nameof(context));
    }
}
