using Microsoft.EntityFrameworkCore;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;
using OndeVou.Infrastructure.Data;

namespace OndeVou.Infrastructure.Repositories;

public class FavoritoRepository : IFavoritoRepository
{
    private readonly AppDbContext _context;

    public FavoritoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteAsync(int usuarioId, int estabelecimentoId)
    {
        return await _context.Favoritos
            .AsNoTracking()
            .AnyAsync(f => f.UsuarioId == usuarioId && f.EstabelecimentoId == estabelecimentoId);
    }

    public async Task<Favorito> CriarAsync(Favorito favorito)
    {
        _context.Favoritos.Add(favorito);
        await _context.SaveChangesAsync();
        return favorito;
    }

    public async Task<bool> RemoverAsync(int usuarioId, int estabelecimentoId)
    {
        var favorito = await _context.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.EstabelecimentoId == estabelecimentoId);

        if (favorito == null)
        {
            return false;
        }

        _context.Favoritos.Remove(favorito);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Favorito>> ListarPorUsuarioIdAsync(int usuarioId)
    {
        return await _context.Favoritos
            .AsNoTracking()
            .Include(f => f.Estabelecimento)
            .Where(f => f.UsuarioId == usuarioId)
            .OrderByDescending(f => f.DataCriacao)
            .ToListAsync();
    }
}