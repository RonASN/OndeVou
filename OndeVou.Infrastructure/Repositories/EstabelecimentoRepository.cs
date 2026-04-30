using Microsoft.EntityFrameworkCore;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;
using OndeVou.Infrastructure.Data;

namespace OndeVou.Infrastructure.Repositories;

public class EstabelecimentoRepository : IEstabelecimentoRepository
{
    private readonly AppDbContext _context;

    public EstabelecimentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Estabelecimento> CriarAsync(Estabelecimento estabelecimento)
    {
        _context.Estabelecimentos.Add(estabelecimento);
        await _context.SaveChangesAsync();
        return estabelecimento;
    }

    public async Task<List<Estabelecimento>> ListarAsync(string? nome, string? categoria, int skip, int take)
    {
        var query = _context.Estabelecimentos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(e => e.Nome.ToLower().Contains(nome.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(e => e.Categoria.ToLower() == categoria.ToLower());
        }

        return await query
            .OrderBy(e => e.Nome)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Estabelecimento?> BuscarPorIdAsync(int id)
    {
        return await _context.Estabelecimentos
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Estabelecimento>> ListarTodosAsync()
    {
        return await _context.Estabelecimentos
            .AsNoTracking()
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<List<Estabelecimento>> ListarPorUsuarioIdAsync(int usuarioId)
    {
        return await _context.Estabelecimentos
            .AsNoTracking()
            .Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }
}
