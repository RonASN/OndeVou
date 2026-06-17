using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IAvaliacaoService
{
    Task<AvaliacaoResponseDto> CriarAsync(CriarAvaliacaoRequestDto request, int usuarioId);
    Task<List<AvaliacaoResponseDto>> ListarPorEstabelecimentoIdAsync(int estabelecimentoId);
    Task<ResumoAvaliacaoResponseDto> ObterResumoPorEstabelecimentoIdAsync(int estabelecimentoId);
}