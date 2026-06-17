using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IEstabelecimentoService
{
    Task<EstabelecimentoResponseDto> CriarAsync(CriarEstabelecimentoRequestDto request, int usuarioId);
    Task<List<EstabelecimentoResponseDto>> ListarAsync(EstabelecimentoFiltroRequestDto filtro);
    Task<EstabelecimentoPaginadoResponseDto> ListarFeedAsync(EstabelecimentoFeedFiltroRequestDto filtro);
    Task<List<EstabelecimentoProximoResponseDto>> ListarProximosAsync(double latitude, double longitude, double raioKm);
    Task<EstabelecimentoResponseDto?> ObterPorIdAsync(int id);
    Task<EstabelecimentoDetalhesResponseDto?> ObterDetalhesAsync(int id, int? usuarioId);
    Task<GeoJsonFeatureCollectionDto> ListarGeoJsonAsync(string? nome = null, string? categoria = null);
    Task<List<EstabelecimentoResponseDto>> ListarPorUsuarioIdAsync(int usuarioId);
}
