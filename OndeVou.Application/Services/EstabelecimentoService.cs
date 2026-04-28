using NetTopologySuite.Geometries;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;

namespace OndeVou.Application.Services;

public class EstabelecimentoService : IEstabelecimentoService
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;

    public EstabelecimentoService(IEstabelecimentoRepository estabelecimentoRepository)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
    }

    public async Task<EstabelecimentoResponseDto> CriarAsync(CriarEstabelecimentoRequestDto request)
    {
        var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        var estabelecimento = new Estabelecimento
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            Categoria = request.Categoria,
            Localizacao = geometryFactory.CreatePoint(new Coordinate(request.Longitude, request.Latitude))
        };

        var resultado = await _estabelecimentoRepository.CriarAsync(estabelecimento);

        return new EstabelecimentoResponseDto
        {
            Id = resultado.Id,
            Nome = resultado.Nome,
            Descricao = resultado.Descricao,
            Categoria = resultado.Categoria,
            Latitude = resultado.Localizacao.Y,
            Longitude = resultado.Localizacao.X
        };
    }
}
