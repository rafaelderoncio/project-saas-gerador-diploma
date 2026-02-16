namespace Project.SaaS.Certfy.Core.Helpers;

public static class FileHelper
{
    public static async Task<string> LoadContentAsync(string path)
    {
        if (!File.Exists(path))
            return default!;

        using var reader = new StreamReader(path);
        return reader.ReadToEnd();
    }
}