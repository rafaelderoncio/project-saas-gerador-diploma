using AutoFixture;
using Project.SaaS.Certfy.Core.Models;

namespace Project.SaaS.Certfy.Test.Fixtures;

public static class CourseFixture
{
    public static CourseModel CreateModel(IFixture fixture, string? id = null)
    {
        return fixture.Build<CourseModel>()
            .With(x => x.Id, id ?? fixture.Create<string>())
            .With(x => x.Name, "Engenharia de Software")
            .With(x => x.Degree, "Bacharelado")
            .With(x => x.TotalWorkload, 3200)
            .With(x => x.AverageApproval, 7.5m)
            .Create();
    }
}
