using NetTopologySuite.Geometries;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Enums;
using OndeVou.Domain.Interfaces;

namespace OndeVou.Application.Services;

public class EstabelecimentoService : IEstabelecimentoService
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public EstabelecimentoService(
        IEstabelecimentoRepository estabelecimentoRepository,
        IUsuarioRepository usuarioRepository)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<EstabelecimentoResponseDto> CriarAsync(CriarEstabelecimentoRequestDto request, int usuarioId)
    {
        var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId);
        if (usuario == null)
        {
            throw new BusinessException("Usuário não encontrado");
        }

        if (usuario.TipoUsuario != TipoUsuario.Empresa)
        {
            throw new BusinessException("Apenas usuários do tipo Empresa podem cadastrar estabelecimentos");
        }

        var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        var estabelecimento = new Estabelecimento
        {
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao.Trim(),
            Categoria = request.Categoria.Trim(),
            Localizacao = geometryFactory.CreatePoint(new Coordinate(request.Longitude, request.Latitude)),
            UsuarioId = usuarioId
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

    public async Task<List<EstabelecimentoResponseDto>> ListarAsync(EstabelecimentoFiltroRequestDto filtro)
    {
        var estabelecimentos = await _estabelecimentoRepository.ListarAsync(
            filtro.Nome,
            filtro.Categoria,
            filtro.Skip,
            filtro.Take);

        return estabelecimentos.Select(e => new EstabelecimentoResponseDto
        {
            Id = e.Id,
            Nome = e.Nome,
            Descricao = e.Descricao,
            Categoria = e.Categoria,
            Latitude = e.Localizacao.Y,
            Longitude = e.Localizacao.X
        }).ToList();
    }

    public async Task<EstabelecimentoResponseDto?> ObterPorIdAsync(int id)
    {
        var estabelecimento = await _estabelecimentoRepository.BuscarPorIdAsync(id);

        if (estabelecimento == null)
            return null;

        return new EstabelecimentoResponseDto
        {
            Id = estabelecimento.Id,
            Nome = estabelecimento.Nome,
            Descricao = estabelecimento.Descricao,
            Categoria = estabelecimento.Categoria,
            Latitude = estabelecimento.Localizacao.Y,
            Longitude = estabelecimento.Localizacao.X
        };
    }

    public async Task<GeoJsonFeatureCollectionDto> ListarGeoJsonAsync()
    {
        var estabelecimentos = await _estabelecimentoRepository.ListarTodosAsync();

        var features = estabelecimentos.Select(e => new GeoJsonFeatureDto
        {
            Type = "Feature",
            Geometry = new GeoJsonGeometryDto
            {
                Type = "Point",
                Coordinates = new[] { e.Localizacao.X, e.Localizacao.Y }
            },
            Properties = new Dictionary<string, object>
            {
                { "id", e.Id },
                { "nome", e.Nome },
                { "categoria", e.Categoria },
                { "descricao", e.Descricao }
            }
        }).ToList();

        return new GeoJsonFeatureCollectionDto
        {
            Type = "FeatureCollection",
            Features = features
        };
    }

    public async Task<List<EstabelecimentoResponseDto>> ListarPorUsuarioIdAsync(int usuarioId)
    {
        var estabelecimentos = await _estabelecimentoRepository.ListarPorUsuarioIdAsync(usuarioId);

        return estabelecimentos.Select(e => new EstabelecimentoResponseDto
        {
            Id = e.Id,
            Nome = e.Nome,
            Descricao = e.Descricao,
            Categoria = e.Categoria,
            Latitude = e.Localizacao.Y,
            Longitude = e.Localizacao.X
        }).ToList();
    }
}
