using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.SaaS.Certfy.Core.Exceptions;

namespace Project.SaaS.Certfy.Core.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            @$"Error: {exception.Message}
            Trace: {exception.StackTrace}"
        );

        var details = exception is BaseException baseException ? 
            new ProblemDetails
            {
                Status = (int)baseException.Status,
                Title = baseException.Title,
                Detail = baseException.Detail
            }:
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro Interno",
                Detail = "Ocorreu um erro interno no servidor."
            };

        httpContext.Response.StatusCode = details.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
        
        return true;
    }
}
