using System.ComponentModel.DataAnnotations;

namespace OndeVou.Application.DTOs.Request;

public class LoginRequestDto
{
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;
}
