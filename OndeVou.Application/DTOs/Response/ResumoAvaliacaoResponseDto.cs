namespace OndeVou.Application.DTOs.Response;

public class ResumoAvaliacaoResponseDto
{
    public int EstabelecimentoId { get; set; }
    public double MediaNotas { get; set; }
    public int QuantidadeAvaliacoes { get; set; }
}