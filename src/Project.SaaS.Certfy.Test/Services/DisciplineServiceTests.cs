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

public class DisciplineServiceTests
{
    private readonly IFixture _fixture = AutoNSubstituteFixture.Create();

    [Fact]
    public async Task GetDisciplineAsync_WhenDisciplineExists_ShouldReturnMappedResponse()
    {
        var repository = _fixture.Create<IDisciplineRepository>();
        var service = new DisciplineService(repository);
        var disciplineId = _fixture.Create<string>();

        var model = DisciplineFixture.CreateModel(_fixture, disciplineId);

        repository.GetDisciplineAsync(disciplineId).Returns(model);

        var response = await service.GetDisciplineAsync(disciplineId);

        Assert.NotNull(response);
        Assert.Equal(model.Id, response.DisciplineId);
        Assert.Equal(model.Name, response.Name);
        Assert.Equal(model.Description, response.Description);
        Assert.Equal(model.Workload, response.Workload);
        Assert.Equal(model.AverageApproval, response.AverageApproval);
        Assert.Equal(model.Types, response.Types);
    }

    [Fact]
    public async Task GetDisciplineAsync_WhenDisciplineDoesNotExist_ShouldThrowBaseException()
    {
        var repository = _fixture.Create<IDisciplineRepository>();
        var service = new DisciplineService(repository);
        var disciplineId = _fixture.Create<string>();

        repository.GetDisciplineAsync(disciplineId).Returns((DisciplineModel?)null);

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GetDisciplineAsync(disciplineId));

        Assert.Equal(HttpStatusCode.NotFound, exception.Status);
        Assert.Equal("Erro Disciplina", exception.Title);
        Assert.Contains(disciplineId, exception.Detail);
    }

    [Fact]
    public async Task GetDisciplinesAsync_WhenPaginationIsInvalid_ShouldUseDefaultValues()
    {
        var repository = _fixture.Create<IDisciplineRepository>();
        var service = new DisciplineService(repository);
        var request = new PaginationRequest(page: -1, size: 0);
        var models = Enumerable.Range(1, 2).Select(_ => DisciplineFixture.CreateModel(_fixture)).ToList();

        repository.GetDisciplineAsync(10, 1).Returns(models);

        var response = await service.GetDisciplinesAsync(request);

        await repository.Received(1).GetDisciplineAsync(10, 1);
        Assert.Equal(1, request.Page);
        Assert.Equal(10, request.Size);
        Assert.Equal(models.Count, response.Count);
    }

    [Fact]
    public async Task GetDisciplinesAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        var repository = _fixture.Create<IDisciplineRepository>();
        var service = new DisciplineService(repository);
        var request = new PaginationRequest(page: 1, size: 10);

        repository.GetDisciplineAsync(request.Size, request.Page).Returns(Enumerable.Empty<DisciplineModel>());

        var response = await service.GetDisciplinesAsync(request);

        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
