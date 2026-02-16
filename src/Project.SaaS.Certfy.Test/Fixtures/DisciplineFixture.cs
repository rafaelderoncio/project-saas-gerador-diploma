using AutoFixture;
using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Test.Fixtures;

public static class DisciplineFixture
{
    public static DisciplineModel CreateModel(IFixture fixture, string? id = null)
    {
        return fixture.Build<DisciplineModel>()
            .With(x => x.Id, id ?? fixture.Create<string>())
            .With(x => x.Name, "Estruturas de Dados")
            .With(x => x.Description, "Fundamentos de estruturas lineares e não-lineares.")
            .With(x => x.Types, ["Tecnologia", "Dados"])
            .With(x => x.Workload, 80)
            .With(x => x.AverageApproval, 7.0m)
            .Create();
    }
}
