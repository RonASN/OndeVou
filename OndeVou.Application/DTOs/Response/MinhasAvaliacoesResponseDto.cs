namespace OndeVou.Application.DTOs.Response;

public class MinhasAvaliacoesResponseDto
{
    public int Id { get; set; }
    public int EstabelecimentoId { get; set; }
    public string EstabelecimentoNome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
