using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IEstabelecimentoService
{
    Task<EstabelecimentoResponseDto> CriarAsync(CriarEstabelecimentoRequestDto request, int usuarioId);
}
