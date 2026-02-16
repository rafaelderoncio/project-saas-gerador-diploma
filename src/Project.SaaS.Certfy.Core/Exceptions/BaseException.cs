using System.Net;

namespace Project.SaaS.Certfy.Core.Exceptions;

public class BaseException : Exception
{
    public string Title { get; private set; } = "Erro interno";
    public string Detail { get; private set; } = "Erro ao processar requisição.";
    public HttpStatusCode Status { get; private set; } = HttpStatusCode.InternalServerError;

    public BaseException(string exceptionMessage) : base(exceptionMessage)
    {
    }

    public BaseException(string exceptionMessage, string detail, string title, HttpStatusCode status) : base(exceptionMessage)
    {
        Title = title;
        Detail = detail;
        Status = status;
    }

    public BaseException(string detail, string title, HttpStatusCode status) : base(detail)
    {
        Title = title;
        Detail = detail;
        Status = status;
    }
}
