using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CriarAsync(CriarUsuarioRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<List<UsuarioResponseDto>> ListarAsync(PaginacaoRequestDto paginacao);
    Task<UsuarioResponseDto?> ObterPorIdAsync(int id);
}
