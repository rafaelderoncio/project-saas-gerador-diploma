namespace Project.SaaS.Certfy.Domain.Requests;

/// <summary>
/// Requisição para geração de certificado/diploma.
/// </summary>
public record CertificateRequest
{
    /// <summary>
    /// Identificador da instituição de ensino.
    /// </summary>
    public string InstitutionId { get; set; } = default!;

    /// <summary>
    /// Data de conclusão do curso pelo aluno.
    /// </summary>
    public DateTime ConclusionDate { get; set; }

    /// <summary>
    /// Dados de identificação do aluno.
    /// </summary>
    public StudentCertificateRequest Student { get; set; } = default!;

    /// <summary>
    /// Dados do curso e das disciplinas cursadas.
    /// </summary>
    public CourseCertificateRequest Course { get; set; } = default!;

    /// <summary>
    /// Dados das assinaturas responsáveis no documento.
    /// </summary>
    public SignatureCertificateRequest Signature { get; set; } = default!;
}

/// <summary>
/// Dados do aluno no certificado.
/// </summary>
public record StudentCertificateRequest
{
    /// <summary>
    /// Nome completo do aluno.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Número do documento civil informado.
    /// </summary>
    public string DocumentNumber { get; set; } = default!;

    /// <summary>
    /// Tipo do documento (ex.: CPF, RG, CNH).
    /// </summary>
    public string DocumentType { get; set; } = default!;

    /// <summary>
    /// Matrícula acadêmica do aluno.
    /// </summary>
    public string Registration { get; set; } = default!;
}

/// <summary>
/// Dados do curso no certificado.
/// </summary>
public record CourseCertificateRequest
{
    /// <summary>
    /// Identificador do curso.
    /// </summary>
    public string CourseId { get; set; } = default!;

    /// <summary>
    /// Lista de disciplinas vinculadas ao curso.
    /// </summary>
    public List<DisciplineCertificateRequest> Disciplines { get; set; } = [];
}

/// <summary>
/// Dados da disciplina cursada no histórico do certificado.
/// </summary>
public record DisciplineCertificateRequest
{
    /// <summary>
    /// Identificador da disciplina.
    /// </summary>
    public string DisciplineId { get; set; } = default!;

    /// <summary>
    /// Período letivo cursado (ex.: 2025.1).
    /// </summary>
    public string Period { get; set; } = default!;

    /// <summary>
    /// Média final atingida na disciplina.
    /// </summary>
    public decimal Average { get; set; }

    /// <summary>
    /// Indica se a disciplina foi dispensada.
    /// </summary>
    public bool HasDispensed { get; set; }
}

/// <summary>
/// Dados de assinatura para composição do certificado.
/// </summary>
public record SignatureCertificateRequest
{
    /// <summary>
    /// Nome do responsável acadêmico (ex.: diretor).
    /// </summary>
    public string DeersPersonName { get; set; } = default!;

    /// <summary>
    /// Nome do responsável administrativo.
    /// </summary>
    public string AdministrativePersonName { get; set; } = default!;
}
