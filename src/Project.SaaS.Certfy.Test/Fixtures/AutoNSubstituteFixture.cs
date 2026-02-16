using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Project.SaaS.Certfy.Test.Fixtures;

public static class AutoNSubstituteFixture
{
    public static IFixture Create()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization
        {
            ConfigureMembers = true
        });

        return fixture;
    }
}
