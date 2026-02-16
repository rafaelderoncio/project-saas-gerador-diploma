using AutoFixture;
using Project.SaaS.Certfy.Domain.Requests;

namespace Project.SaaS.Certfy.Test.Fixtures;

public static class CertificateRequestFixture
{
    public static CertificateRequest CreateValidRequest(IFixture fixture)
    {
        return new CertificateRequest
        {
            InstitutionId = "inst-001",
            ConclusionDate = DateTime.UtcNow,
            Student = new StudentCertificateRequest
            {
                Name = $"Aluno {fixture.Create<string>()[..8]}",
                DocumentNumber = "12345678901",
                DocumentType = "CPF",
                Registration = "2023001234"
            },
            Course = new CourseCertificateRequest
            {
                CourseId = "course-001",
                Disciplines =
                [
                    new DisciplineCertificateRequest
                    {
                        DisciplineId = "disc-001",
                        Period = "2025/1",
                        Average = 8.5m,
                        HasDispensed = false
                    },
                    new DisciplineCertificateRequest
                    {
                        DisciplineId = "disc-002",
                        Period = "2025/2",
                        Average = 9.0m,
                        HasDispensed = false
                    }
                ]
            },
            Signature = new SignatureCertificateRequest
            {
                DeersPersonName = "Diretor Acadêmico",
                AdministrativePersonName = "Secretário Geral"
            }
        };
    }
}
