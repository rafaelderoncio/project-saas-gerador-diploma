using System.ComponentModel.DataAnnotations;

namespace Project.SaaS.Certfy.Domain.Enums;

/// <summary>
/// Tipos de documento aceitos para identificação do aluno.
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// Cadastro de Pessoa Física.
    /// </summary>
    [Display(Name = "CPF")]
    CPF,

    /// <summary>
    /// Registro Geral.
    /// </summary>
    [Display(Name = "RG")]
    RG,

    /// <summary>
    /// Carteira Nacional de Habilitação.
    /// </summary>
    [Display(Name = "CNH")]
    CNH,
}
