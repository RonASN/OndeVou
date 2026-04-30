using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;

namespace OndeVou.Application.Interfaces;

public interface IEstabelecimentoService
{
    Task<EstabelecimentoResponseDto> CriarAsync(CriarEstabelecimentoRequestDto request, int usuarioId);
    Task<List<EstabelecimentoResponseDto>> ListarAsync(EstabelecimentoFiltroRequestDto filtro);
    Task<EstabelecimentoResponseDto?> ObterPorIdAsync(int id);
    Task<GeoJsonFeatureCollectionDto> ListarGeoJsonAsync();
    Task<List<EstabelecimentoResponseDto>> ListarPorUsuarioIdAsync(int usuarioId);
}
