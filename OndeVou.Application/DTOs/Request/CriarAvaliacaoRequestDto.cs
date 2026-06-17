using System.ComponentModel.DataAnnotations;

namespace OndeVou.Application.DTOs.Request;

public class CriarAvaliacaoRequestDto
{
    [Required(ErrorMessage = "A nota é obrigatória")]
    [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5")]
    public int Nota { get; set; }

    [Required(ErrorMessage = "O comentário é obrigatório")]
    [MaxLength(1000, ErrorMessage = "O comentário deve ter no máximo 1000 caracteres")]
    public string Comentario { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estabelecimento é obrigatório")]
    public int EstabelecimentoId { get; set; }
}