using System.Net;
using AutoFixture;
using NSubstitute;
using Project.SaaS.Certfy.Core.Exceptions;
using Project.SaaS.Certfy.Core.Models;
using Project.SaaS.Certfy.Core.Repositories.Interfaces;
using Project.SaaS.Certfy.Core.Services;
using Project.SaaS.Certfy.Domain.Requests;
using Project.SaaS.Certfy.Test.Fixtures;

namespace Project.SaaS.Certfy.Test.Services;

public class InstitutionServiceTests
{
    private readonly IFixture _fixture = AutoNSubstituteFixture.Create();

    [Fact]
    public async Task GetInstitutionAsync_WhenInstitutionExists_ShouldReturnMappedResponse()
    {
        var repository = _fixture.Create<IInstitutionRepository>();
        var service = new InstitutionService(repository);
        var institutionId = _fixture.Create<string>();

        var model = InstitutionFixture.CreateModel(_fixture, institutionId);

        repository.GetInstitutionAsync(institutionId).Returns(model);

        var response = await service.GetInstitutionAsync(institutionId);

        Assert.NotNull(response);
        Assert.Equal(model.Id, response.InstitutionId);
        Assert.Equal(model.Name, response.Name);
        Assert.Equal(model.Acronym, response.Acronym);
        Assert.Equal(model.City, response.City);
        Assert.Equal(model.State, response.State);
        Assert.Equal(model.Type, response.Type);
    }

    [Fact]
    public async Task GetInstitutionAsync_WhenInstitutionDoesNotExist_ShouldThrowBaseException()
    {
        var repository = _fixture.Create<IInstitutionRepository>();
        var service = new InstitutionService(repository);
        var institutionId = _fixture.Create<string>();

        repository.GetInstitutionAsync(institutionId).Returns((InstitutionModel?)null);

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GetInstitutionAsync(institutionId));

        Assert.Equal(HttpStatusCode.NotFound, exception.Status);
        Assert.Equal("Erro Disciplina", exception.Title);
        Assert.Contains(institutionId, exception.Detail);
    }

    [Fact]
    public async Task GetInstitutionsAsync_WhenPaginationIsInvalid_ShouldUseDefaultValues()
    {
        var repository = _fixture.Create<IInstitutionRepository>();
        var service = new InstitutionService(repository);
        var request = new PaginationRequest(page: 0, size: -10);
        var models = Enumerable.Range(1, 2).Select(_ => InstitutionFixture.CreateModel(_fixture)).ToList();

        repository.GetInstitutionsAsync(10, 1).Returns(models);

        var response = await service.GetInstitutionsAsync(request);

        await repository.Received(1).GetInstitutionsAsync(10, 1);
        Assert.Equal(1, request.Page);
        Assert.Equal(10, request.Size);
        Assert.Equal(models.Count, response.Count);
    }

    [Fact]
    public async Task GetInstitutionsAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        var repository = _fixture.Create<IInstitutionRepository>();
        var service = new InstitutionService(repository);
        var request = new PaginationRequest(page: 1, size: 10);

        repository.GetInstitutionsAsync(request.Size, request.Page).Returns(Enumerable.Empty<InstitutionModel>());

        var response = await service.GetInstitutionsAsync(request);

        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
