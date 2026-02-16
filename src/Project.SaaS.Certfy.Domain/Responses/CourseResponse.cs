namespace Project.SaaS.Certfy.Domain.Responses;

public record CourseResponse
{
    public string CourseId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public int TotalWorkload { get; set; }
    public decimal AverageApproval { get; set; }
}
