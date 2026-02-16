namespace Project.SaaS.Certfy.Core.Models;

public class DisciplineModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string[] Types { get; set; } = default!;
    public int Workload { get; set; }
    public decimal AverageApproval { get; set; }
}