namespace Project.SaaS.Certfy.Core.Models;

public class CourseModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public int TotalWorkload { get; set; } = 24;
    public decimal AverageApproval { get; set; } = 7.0m;
}
