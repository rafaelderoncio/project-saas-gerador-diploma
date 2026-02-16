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

public class CourseServiceTests
{
    private readonly IFixture _fixture = AutoNSubstituteFixture.Create();

    [Fact]
    public async Task GetCourseAsync_WhenCourseExists_ShouldReturnMappedResponse()
    {
        var repository = _fixture.Create<ICourseRepository>();
        var service = new CourseService(repository);
        var courseId = _fixture.Create<string>();

        var model = CourseFixture.CreateModel(_fixture, courseId);

        repository.GetCourseAsync(courseId).Returns(model);

        var response = await service.GetCourseAsync(courseId);

        Assert.NotNull(response);
        Assert.Equal(model.Id, response.CourseId);
        Assert.Equal(model.Name, response.Name);
        Assert.Equal(model.Degree, response.Degree);
        Assert.Equal(model.TotalWorkload, response.TotalWorkload);
        Assert.Equal(model.AverageApproval, response.AverageApproval);
    }

    [Fact]
    public async Task GetCourseAsync_WhenCourseDoesNotExist_ShouldThrowBaseException()
    {
        var repository = _fixture.Create<ICourseRepository>();
        var service = new CourseService(repository);
        var courseId = _fixture.Create<string>();

        repository.GetCourseAsync(courseId).Returns((CourseModel?)null);

        var exception = await Assert.ThrowsAsync<BaseException>(() => service.GetCourseAsync(courseId));

        Assert.Equal(HttpStatusCode.NotFound, exception.Status);
        Assert.Equal("Erro Curso", exception.Title);
        Assert.Contains(courseId, exception.Detail);
    }

    [Fact]
    public async Task GetCoursesAsync_WhenPaginationIsInvalid_ShouldUseDefaultValues()
    {
        var repository = _fixture.Create<ICourseRepository>();
        var service = new CourseService(repository);
        var request = new PaginationRequest(page: 0, size: 0);
        var models = Enumerable.Range(1, 3).Select(_ => CourseFixture.CreateModel(_fixture)).ToList();

        repository.GetCoursesAsync(10, 1).Returns(models);

        var response = await service.GetCoursesAsync(request);

        await repository.Received(1).GetCoursesAsync(10, 1);
        Assert.Equal(1, request.Page);
        Assert.Equal(10, request.Size);
        Assert.Equal(models.Count, response.Count);
    }

    [Fact]
    public async Task GetCoursesAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        var repository = _fixture.Create<ICourseRepository>();
        var service = new CourseService(repository);
        var request = new PaginationRequest(page: 1, size: 10);

        repository.GetCoursesAsync(request.Size, request.Page).Returns(Enumerable.Empty<CourseModel>());

        var response = await service.GetCoursesAsync(request);

        Assert.NotNull(response);
        Assert.Empty(response);
    }
}
