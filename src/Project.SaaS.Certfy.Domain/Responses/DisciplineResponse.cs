namespace Project.SaaS.Certfy.Domain.Responses;

public record DisciplineResponse
{
    public string DisciplineId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string[] Types { get; set; } = default!;
    public int Workload { get; set; }
    public decimal AverageApproval { get; set; }
}
