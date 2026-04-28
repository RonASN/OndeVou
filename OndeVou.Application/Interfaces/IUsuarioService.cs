using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CriarAsync(CriarUsuarioRequestDto request);
}
