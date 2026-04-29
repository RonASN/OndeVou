namespace OndeVou.Application.DTOs.Response;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TipoUsuario { get; set; }
    public string TipoUsuarioDescricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
