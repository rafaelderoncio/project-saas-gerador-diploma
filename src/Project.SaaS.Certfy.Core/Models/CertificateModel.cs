using Project.SaaS.Certfy.Domain.Enums;

namespace Project.SaaS.Certfy.Core.Models;

/// <summary>
/// Modelo agregado com todos os dados necessários para renderizar o certificado.
/// </summary>
public record class CertificateModel
{
    private readonly static string _id = Guid.NewGuid().ToString();

    /// <summary>
    /// Identificador interno do certificado em memória.
    /// </summary>
    public static string Id => _id;

    /// <summary>
    /// Código/hash de autenticação do certificado.
    /// </summary>
    public string Authentication { get; set; } = default!;

    /// <summary>
    /// Data de conclusão formatada para exibição.
    /// </summary>
    public string ConclusionDate { get; set; } = default!;

    /// <summary>
    /// Dados do aluno.
    /// </summary>
    public StudentCertificateModel Student { get; set; } = default!;

    /// <summary>
    /// Dados da instituição.
    /// </summary>
    public InstitutionCertificateModel Institution { get; set; } = default!;

    /// <summary>
    /// Dados do curso.
    /// </summary>
    public CourseCertificateModel Course { get; set; } = default!;

    /// <summary>
    /// Disciplinas consideradas no certificado.
    /// </summary>
    public List<DisciplineCertificateModel> Disciplines { get; set; } = [];

    /// <summary>
    /// Dados de assinatura.
    /// </summary>
    public SignatureCertificateModel Signature { get; set; } = default!;

    /// <summary>
    /// Situação final do aluno.
    /// </summary>
    public StatusType Status { get; set; } = default!;
}

/// <summary>
/// Dados da instituição para o certificado.
/// </summary>
public record InstitutionCertificateModel
{
    /// <summary>
    /// Identificador da instituição.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Nome da instituição.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Nome de campus/unidade.
    /// </summary>
    public string Campus { get; set; } = default!;
}

/// <summary>
/// Dados do curso para o certificado.
/// </summary>
public record CourseCertificateModel
{
    /// <summary>
    /// Identificador do curso.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Nome do curso.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Grau acadêmico.
    /// </summary>
    public DegreeType Degree { get; set; } = default!;

    /// <summary>
    /// Média mínima para aprovação.
    /// </summary>
    public decimal Avagage { get; set; }
}

/// <summary>
/// Dados do aluno para o certificado.
/// </summary>
public record StudentCertificateModel
{
    /// <summary>
    /// Nome completo do aluno.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Número de documento do aluno.
    /// </summary>
    public string DocumentNumber { get; set; } = default!;

    /// <summary>
    /// Tipo de documento do aluno.
    /// </summary>
    public DocumentType DocumentType { get; set; } = default!;

    /// <summary>
    /// Matrícula do aluno.
    /// </summary>
    public string Registration { get; set; } = default!;
}

/// <summary>
/// Dados de disciplina para composição do certificado.
/// </summary>
public record DisciplineCertificateModel
{
    /// <summary>
    /// Identificador da disciplina.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Nome da disciplina.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Período letivo da disciplina.
    /// </summary>
    public string Period { get; set; } = default!;

    /// <summary>
    /// Média atingida na disciplina.
    /// </summary>
    public decimal Average { get; set; }

    /// <summary>
    /// Carga horária da disciplina.
    /// </summary>
    public int Workload { get; set; }

    /// <summary>
    /// Status acadêmico da disciplina.
    /// </summary>
    public StatusType Status { get; set; } = default!;
}

/// <summary>
/// Dados de assinatura e local/data de emissão.
/// </summary>
public record SignatureCertificateModel
{
    /// <summary>
    /// Nome do responsável acadêmico.
    /// </summary>
    public string DeersPersonName { get; set; } = default!;

    /// <summary>
    /// Nome do responsável administrativo.
    /// </summary>
    public string AdmnistrativePersonName { get; set; } = default!;

    /// <summary>
    /// Local de emissão.
    /// </summary>
    public string Location { get; set; } = default!;

    /// <summary>
    /// Data de emissão.
    /// </summary>
    public string Date { get; set; } = default!;
}
