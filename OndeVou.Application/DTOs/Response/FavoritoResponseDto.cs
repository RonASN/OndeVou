namespace OndeVou.Application.DTOs.Response;

public class FavoritoResponseDto
{
    public int EstabelecimentoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime DataCriacao { get; set; }
}