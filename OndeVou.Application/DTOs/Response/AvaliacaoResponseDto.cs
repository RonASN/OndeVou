namespace OndeVou.Application.DTOs.Response;

public class AvaliacaoResponseDto
{
    public int Id { get; set; }
    public int Nota { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public int EstabelecimentoId { get; set; }
}