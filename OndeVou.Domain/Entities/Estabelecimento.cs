using NetTopologySuite.Geometries;

namespace OndeVou.Domain.Entities;

public class Estabelecimento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public Point Localizacao { get; set; } = null!;
}
