using System.ComponentModel.DataAnnotations;

namespace OndeVou.Application.DTOs.Request;

public class CriarUsuarioRequestDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(200, ErrorMessage = "O email deve ter no máximo 200 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
    [Range(1, 2, ErrorMessage = "Tipo de usuário inválido. Use 1 para Comum ou 2 para Empresa")]
    public int TipoUsuario { get; set; }
}
