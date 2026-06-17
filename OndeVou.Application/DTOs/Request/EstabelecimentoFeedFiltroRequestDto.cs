namespace OndeVou.Application.DTOs.Request;

public class EstabelecimentoFeedFiltroRequestDto
{
    public string? Nome { get; set; }
    public string? Categoria { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrdenarPor { get; set; } = "Nome";
}