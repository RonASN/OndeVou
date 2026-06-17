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
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly IFavoritoRepository _favoritoRepository;

    public EstabelecimentoService(
        IEstabelecimentoRepository estabelecimentoRepository,
        IUsuarioRepository usuarioRepository,
        IAvaliacaoRepository avaliacaoRepository,
        IFavoritoRepository favoritoRepository)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
        _usuarioRepository = usuarioRepository;
        _avaliacaoRepository = avaliacaoRepository;
        _favoritoRepository = favoritoRepository;
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
            DataCriacao = DateTime.UtcNow,
            UsuarioId = usuarioId
        };

        var resultado = await _estabelecimentoRepository.CriarAsync(estabelecimento);

        return MapearEstabelecimento(resultado);
    }

    public async Task<List<EstabelecimentoResponseDto>> ListarAsync(EstabelecimentoFiltroRequestDto filtro)
    {
        var estabelecimentos = await _estabelecimentoRepository.ListarAsync(
            filtro.Nome,
            filtro.Categoria,
            filtro.Skip,
            filtro.Take);

        return estabelecimentos.Select(MapearEstabelecimento).ToList();
    }

    public async Task<EstabelecimentoPaginadoResponseDto> ListarFeedAsync(EstabelecimentoFeedFiltroRequestDto filtro)
    {
        var page = filtro.Page < 1 ? 1 : filtro.Page;
        var pageSize = filtro.PageSize < 1 ? 10 : filtro.PageSize;

        var total = await _estabelecimentoRepository.ContarAsync(filtro.Nome, filtro.Categoria);
        var itens = await _estabelecimentoRepository.ListarOrdenadoAsync(
            filtro.Nome,
            filtro.Categoria,
            page,
            pageSize,
            filtro.OrdenarPor);

        var totalPaginas = total == 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize);

        return new EstabelecimentoPaginadoResponseDto
        {
            TotalRegistros = total,
            TotalPaginas = totalPaginas,
            PaginaAtual = page,
            Itens = itens.Select(MapearEstabelecimento).ToList()
        };
    }

    public async Task<List<EstabelecimentoProximoResponseDto>> ListarProximosAsync(double latitude, double longitude, double raioKm)
    {
        if (raioKm <= 0)
        {
            throw new BusinessException("O raio deve ser maior que zero");
        }

        var proximos = await _estabelecimentoRepository.ListarProximosAsync(latitude, longitude, raioKm);

        return proximos.Select(x => new EstabelecimentoProximoResponseDto
        {
            Id = x.Estabelecimento.Id,
            Nome = x.Estabelecimento.Nome,
            Descricao = x.Estabelecimento.Descricao,
            Categoria = x.Estabelecimento.Categoria,
            Latitude = x.Estabelecimento.Localizacao.Y,
            Longitude = x.Estabelecimento.Localizacao.X,
            DistanciaKm = Math.Round(x.DistanciaKm, 2)
        }).ToList();
    }

    public async Task<EstabelecimentoResponseDto?> ObterPorIdAsync(int id)
    {
        var estabelecimento = await _estabelecimentoRepository.BuscarPorIdAsync(id);

        if (estabelecimento == null)
            return null;

        return MapearEstabelecimento(estabelecimento);
    }

    public async Task<EstabelecimentoDetalhesResponseDto?> ObterDetalhesAsync(int id, int? usuarioId)
    {
        var estabelecimento = await _estabelecimentoRepository.BuscarPorIdAsync(id);
        if (estabelecimento == null)
        {
            return null;
        }

        var (media, quantidade) = await _avaliacaoRepository.ObterResumoPorEstabelecimentoIdAsync(id);

        var favorito = false;
        if (usuarioId.HasValue)
        {
            favorito = await _favoritoRepository.ExisteAsync(usuarioId.Value, id);
        }

        return new EstabelecimentoDetalhesResponseDto
        {
            Id = estabelecimento.Id,
            Nome = estabelecimento.Nome,
            Descricao = estabelecimento.Descricao,
            Categoria = estabelecimento.Categoria,
            Latitude = estabelecimento.Localizacao.Y,
            Longitude = estabelecimento.Localizacao.X,
            MediaAvaliacoes = media,
            QuantidadeAvaliacoes = quantidade,
            FavoritadoPeloUsuario = favorito
        };
    }

    public async Task<GeoJsonFeatureCollectionDto> ListarGeoJsonAsync(string? nome = null, string? categoria = null)
    {
        var estabelecimentos = await _estabelecimentoRepository.ListarTodosFiltradosAsync(nome, categoria);

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

        return estabelecimentos.Select(MapearEstabelecimento).ToList();
    }

    private static EstabelecimentoResponseDto MapearEstabelecimento(Estabelecimento e)
    {
        return new EstabelecimentoResponseDto
        {
            Id = e.Id,
            Nome = e.Nome,
            Descricao = e.Descricao,
            Categoria = e.Categoria,
            Latitude = e.Localizacao.Y,
            Longitude = e.Localizacao.X
        };
    }
}
