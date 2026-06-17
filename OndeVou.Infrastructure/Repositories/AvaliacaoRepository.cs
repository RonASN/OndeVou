using Microsoft.EntityFrameworkCore;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;
using OndeVou.Infrastructure.Data;

namespace OndeVou.Infrastructure.Repositories;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly AppDbContext _context;

    public AvaliacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteAsync(int usuarioId, int estabelecimentoId)
    {
        return await _context.Avaliacoes
            .AsNoTracking()
            .AnyAsync(a => a.UsuarioId == usuarioId && a.EstabelecimentoId == estabelecimentoId);
    }

    public async Task<Avaliacao> CriarAsync(Avaliacao avaliacao)
    {
        _context.Avaliacoes.Add(avaliacao);
        await _context.SaveChangesAsync();
        return avaliacao;
    }

    public async Task<List<Avaliacao>> ListarPorEstabelecimentoIdAsync(int estabelecimentoId)
    {
        return await _context.Avaliacoes
            .AsNoTracking()
            .Include(a => a.Usuario)
            .Where(a => a.EstabelecimentoId == estabelecimentoId)
            .OrderByDescending(a => a.DataCriacao)
            .ToListAsync();
    }

    public async Task<(double Media, int Quantidade)> ObterResumoPorEstabelecimentoIdAsync(int estabelecimentoId)
    {
        var query = _context.Avaliacoes
            .AsNoTracking()
            .Where(a => a.EstabelecimentoId == estabelecimentoId);

        var quantidade = await query.CountAsync();
        if (quantidade == 0)
        {
            return (0d, 0);
        }

        var media = await query.AverageAsync(a => a.Nota);
        return (Math.Round(media, 2), quantidade);
    }
}