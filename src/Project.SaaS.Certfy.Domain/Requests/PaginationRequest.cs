namespace Project.SaaS.Certfy.Domain.Requests;

public class PaginationRequest(int page = 1, int size = 10)
{
    public int Page { get; set; } = page <= 0 ? 1 : page;
    public int Size { get; set; } = size <= 0 ? 10 : size;
}

