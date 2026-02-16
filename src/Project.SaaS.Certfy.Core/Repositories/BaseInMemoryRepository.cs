using System;
using System.Text.Json;

namespace Project.SaaS.Certfy.Core.Repositories;

public abstract class BaseInMemoryRepository<TModel> where TModel : class
{
    protected List<TModel> _data = [];

    protected void LoadAsync()
    {
        var prefix = typeof(TModel).Name.Replace("Model", string.Empty).ToLower();
        var path = Path.Combine(AppContext.BaseDirectory, "Repositories", "DataFiles", $"{prefix}.json");
        var content = File.ReadAllText(path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _data = JsonSerializer.Deserialize<List<TModel>>(content, options) ?? [];
    }
}
