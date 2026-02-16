namespace Project.SaaS.Certfy.Domain.Responses;

public record InstitutionResponse
{
    public string InstitutionId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Acronym { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Type { get; set; } = default!;
}
