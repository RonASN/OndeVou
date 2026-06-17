using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IFavoritoService
{
    Task AdicionarAsync(AdicionarFavoritoRequestDto request, int usuarioId);
    Task<bool> RemoverAsync(int usuarioId, int estabelecimentoId);
    Task<List<FavoritoResponseDto>> ListarMeusAsync(int usuarioId);
}