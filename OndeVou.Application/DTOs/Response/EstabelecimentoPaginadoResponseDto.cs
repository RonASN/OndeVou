namespace OndeVou.Application.DTOs.Response;

public class EstabelecimentoPaginadoResponseDto
{
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public int PaginaAtual { get; set; }
    public List<EstabelecimentoResponseDto> Itens { get; set; } = new();
}