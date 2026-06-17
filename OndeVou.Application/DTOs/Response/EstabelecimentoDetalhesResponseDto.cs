namespace OndeVou.Application.DTOs.Response;

public class EstabelecimentoDetalhesResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double MediaAvaliacoes { get; set; }
    public int QuantidadeAvaliacoes { get; set; }
    public bool FavoritadoPeloUsuario { get; set; }
}