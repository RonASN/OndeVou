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
        // Buscar usuário
        var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId);
        if (usuario == null)
        {
            throw new BusinessException("Usuário não encontrado");
        }

        // Validar se o usuário é do tipo Empresa
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
}
