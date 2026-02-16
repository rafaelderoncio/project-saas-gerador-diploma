namespace Project.SaaS.Certfy.Domain.Requests;

public record CertificateRequest
{
    public string InstitutionId { get; set; } = default!;
    public DateTime ConclusionDate { get; set; }
    public StudentCertificateRequest Student { get; set; } = default!;
    public CourseCertificateRequest Course { get; set; } = default!;
    public SignatureCertificateRequest Signature { get; set; } = default!;
}

public record StudentCertificateRequest
{
    public string Name { get; set; } = default!;
    public string DocumentNumber { get; set; } = default!;
    public string DocumentType { get; set; } = default!;
    public string Registration { get; set; } = default!;
}

public record CourseCertificateRequest
{
    public string CourseId { get; set; } = default!;
    public List<DisciplineCertificateRequest> Disciplines { get; set; } = [];
}

public record DisciplineCertificateRequest
{
    public string DisciplineId { get; set; } = default!;
    public string Period { get; set; } = default!;
    public decimal Average { get; set; }
    public bool HasDispensed { get; set; }
}

public record SignatureCertificateRequest
{
    public string DeersPersonName { get; set; } = default!;
    public string AdministrativePersonName { get; set; } = default!;
}