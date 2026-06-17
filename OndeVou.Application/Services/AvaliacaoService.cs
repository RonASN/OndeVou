using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;

namespace OndeVou.Application.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;

    public AvaliacaoService(
        IAvaliacaoRepository avaliacaoRepository,
        IEstabelecimentoRepository estabelecimentoRepository)
    {
        _avaliacaoRepository = avaliacaoRepository;
        _estabelecimentoRepository = estabelecimentoRepository;
    }

    public async Task<AvaliacaoResponseDto> CriarAsync(CriarAvaliacaoRequestDto request, int usuarioId)
    {
        if (request.Nota < 1 || request.Nota > 5)
        {
            throw new BusinessException("A nota deve estar entre 1 e 5");
        }

        var estabelecimento = await _estabelecimentoRepository.BuscarPorIdAsync(request.EstabelecimentoId);
        if (estabelecimento == null)
        {
            throw new BusinessException("Estabelecimento não encontrado");
        }

        var existe = await _avaliacaoRepository.ExisteAsync(usuarioId, request.EstabelecimentoId);
        if (existe)
        {
            throw new BusinessException("Usuário já avaliou este estabelecimento");
        }

        var avaliacao = new Avaliacao
        {
            Nota = request.Nota,
            Comentario = request.Comentario.Trim(),
            DataCriacao = DateTime.UtcNow,
            UsuarioId = usuarioId,
            EstabelecimentoId = request.EstabelecimentoId
        };

        var resultado = await _avaliacaoRepository.CriarAsync(avaliacao);

        return new AvaliacaoResponseDto
        {
            Id = resultado.Id,
            Nota = resultado.Nota,
            Comentario = resultado.Comentario,
            DataCriacao = resultado.DataCriacao,
            UsuarioId = resultado.UsuarioId,
            EstabelecimentoId = resultado.EstabelecimentoId
        };
    }

    public async Task<List<AvaliacaoResponseDto>> ListarPorEstabelecimentoIdAsync(int estabelecimentoId)
    {
        var avaliacoes = await _avaliacaoRepository.ListarPorEstabelecimentoIdAsync(estabelecimentoId);

        return avaliacoes.Select(a => new AvaliacaoResponseDto
        {
            Id = a.Id,
            Nota = a.Nota,
            Comentario = a.Comentario,
            DataCriacao = a.DataCriacao,
            UsuarioId = a.UsuarioId,
            UsuarioNome = a.Usuario.Nome,
            EstabelecimentoId = a.EstabelecimentoId
        }).ToList();
    }

    public async Task<ResumoAvaliacaoResponseDto> ObterResumoPorEstabelecimentoIdAsync(int estabelecimentoId)
    {
        var (media, quantidade) = await _avaliacaoRepository.ObterResumoPorEstabelecimentoIdAsync(estabelecimentoId);

        return new ResumoAvaliacaoResponseDto
        {
            EstabelecimentoId = estabelecimentoId,
            MediaNotas = media,
            QuantidadeAvaliacoes = quantidade
        };
    }
}