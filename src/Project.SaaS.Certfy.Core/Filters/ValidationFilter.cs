using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Project.SaaS.Certfy.Core.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var model = context.Arguments.FirstOrDefault(x => x is T) as T;

        if (model is null)
            return await next(context);

        var validationContext = new ValidationContext(model);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, validationContext, results, true))
        {
            var errors = results
                .GroupBy(r => r.MemberNames.FirstOrDefault() ?? "Model")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(r => r.ErrorMessage!).ToArray()
                );

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
