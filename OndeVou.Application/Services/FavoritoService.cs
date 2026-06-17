using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;

namespace OndeVou.Application.Services;

public class FavoritoService : IFavoritoService
{
    private readonly IFavoritoRepository _favoritoRepository;
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;

    public FavoritoService(
        IFavoritoRepository favoritoRepository,
        IEstabelecimentoRepository estabelecimentoRepository)
    {
        _favoritoRepository = favoritoRepository;
        _estabelecimentoRepository = estabelecimentoRepository;
    }

    public async Task AdicionarAsync(AdicionarFavoritoRequestDto request, int usuarioId)
    {
        var estabelecimento = await _estabelecimentoRepository.BuscarPorIdAsync(request.EstabelecimentoId);
        if (estabelecimento == null)
        {
            throw new BusinessException("Estabelecimento não encontrado");
        }

        var existe = await _favoritoRepository.ExisteAsync(usuarioId, request.EstabelecimentoId);
        if (existe)
        {
            throw new BusinessException("Estabelecimento já favoritado");
        }

        var favorito = new Favorito
        {
            UsuarioId = usuarioId,
            EstabelecimentoId = request.EstabelecimentoId,
            DataCriacao = DateTime.UtcNow
        };

        await _favoritoRepository.CriarAsync(favorito);
    }

    public async Task<bool> RemoverAsync(int usuarioId, int estabelecimentoId)
    {
        return await _favoritoRepository.RemoverAsync(usuarioId, estabelecimentoId);
    }

    public async Task<List<FavoritoResponseDto>> ListarMeusAsync(int usuarioId)
    {
        var favoritos = await _favoritoRepository.ListarPorUsuarioIdAsync(usuarioId);

        return favoritos.Select(f => new FavoritoResponseDto
        {
            EstabelecimentoId = f.EstabelecimentoId,
            Nome = f.Estabelecimento.Nome,
            Categoria = f.Estabelecimento.Categoria,
            Descricao = f.Estabelecimento.Descricao,
            Latitude = f.Estabelecimento.Localizacao.Y,
            Longitude = f.Estabelecimento.Localizacao.X,
            DataCriacao = f.DataCriacao
        }).ToList();
    }
}