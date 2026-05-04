using OndeVou.Domain.Enums;

namespace OndeVou.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public TipoUsuario TipoUsuario { get; set; }
    public DateTime DataCriacao { get; set; }
    public ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
}
