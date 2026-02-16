namespace Project.SaaS.Certfy.Core.Models;

public record class InstitutionModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string Acronym { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Type { get; set; } = default!;
}