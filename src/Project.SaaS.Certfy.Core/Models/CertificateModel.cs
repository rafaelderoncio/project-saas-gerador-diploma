using Project.SaaS.Certfy.Domain.Enums;

namespace Project.SaaS.Certfy.Core.Models;

public record class CertificateModel
{
    private readonly static string _id = Guid.NewGuid().ToString();
    public static string Id => _id;
    public string Authentication { get; set; } = default!;
    public string ConclusionDate { get; set; } = default!;
    public StudentCertificateModel Student { get; set; } = default!;
    public InstitutionCertificateModel Institution { get; set; } = default!;
    public CourseCertificateModel Course { get; set; } = default!;
    public List<DisciplineCertificateModel> Disciplines { get; set; } = [];
    public SignatureCertificateModel Signature { get; set; } = default!;
    public StatusType Status { get; set; } = default!;
}

public record InstitutionCertificateModel
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Campus { get; set; } = default!;
}

public record CourseCertificateModel
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DegreeType Degree { get; set; } = default!;
    public decimal Avagage { get; set; }
}

public record StudentCertificateModel
{
    // public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string DocumentNumber { get; set; } = default!;
    public DocumentType DocumentType { get; set; } = default!;
    public string Registration { get; set; } = default!;
}

public record DisciplineCertificateModel
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Period { get; set; } = default!;
    public decimal Average { get; set; }
    public int Workload { get; set; }
    public StatusType Status { get; set; } = default!;
}

public record SignatureCertificateModel
{
    public string DeersPersonName { get; set; } = default!;
    public string AdmnistrativePersonName { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string Date { get; set; } = default!;
}