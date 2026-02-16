using AutoFixture;
using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Test.Fixtures;

public static class InstitutionFixture
{
    public static InstitutionModel CreateModel(IFixture fixture, string? id = null)
    {
        return fixture.Build<InstitutionModel>()
            .With(x => x.Id, id ?? fixture.Create<string>())
            .With(x => x.Name, "Universidade Exemplo")
            .With(x => x.Acronym, "UEX")
            .With(x => x.City, "São Paulo")
            .With(x => x.State, "SP")
            .With(x => x.Type, "Universidade")
            .Create();
    }
}
