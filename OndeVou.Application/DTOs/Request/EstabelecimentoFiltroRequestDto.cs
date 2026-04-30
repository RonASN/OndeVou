namespace OndeVou.Application.DTOs.Request;

public class EstabelecimentoFiltroRequestDto
{
    public string? Nome { get; set; }
    public string? Categoria { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}
