namespace OndeVou.Application.DTOs.Request;

public class CriarEstabelecimentoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
