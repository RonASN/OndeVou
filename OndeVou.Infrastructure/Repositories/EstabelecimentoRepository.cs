using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
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
        var query = AplicarFiltros(_context.Estabelecimentos.AsNoTracking(), nome, categoria);

        return await query
            .OrderBy(e => e.Nome)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> ContarAsync(string? nome, string? categoria)
    {
        var query = AplicarFiltros(_context.Estabelecimentos.AsNoTracking(), nome, categoria);
        return await query.CountAsync();
    }

    public async Task<List<Estabelecimento>> ListarOrdenadoAsync(string? nome, string? categoria, int page, int pageSize, string? ordenarPor)
    {
        var query = AplicarFiltros(_context.Estabelecimentos.AsNoTracking(), nome, categoria);

        query = (ordenarPor ?? string.Empty).ToLower() switch
        {
            "categoria" => query.OrderBy(e => e.Categoria).ThenBy(e => e.Nome),
            "datacadastro" => query.OrderByDescending(e => e.DataCriacao).ThenBy(e => e.Nome),
            _ => query.OrderBy(e => e.Nome)
        };

        var skip = (page - 1) * pageSize;

        return await query
            .Skip(skip)
            .Take(pageSize)
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

    public async Task<List<Estabelecimento>> ListarTodosFiltradosAsync(string? nome, string? categoria)
    {
        var query = AplicarFiltros(_context.Estabelecimentos.AsNoTracking(), nome, categoria);

        return await query
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

    public async Task<List<(Estabelecimento Estabelecimento, double DistanciaKm)>> ListarProximosAsync(double latitude, double longitude, double raioKm)
    {
        var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var pontoReferencia = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        var raioMetros = raioKm * 1000d;

        var resultado = await _context.Estabelecimentos
            .AsNoTracking()
            .Where(e => e.Localizacao.IsWithinDistance(pontoReferencia, raioMetros / 111320d))
            .Select(e => new
            {
                Estabelecimento = e,
                DistanciaKm = e.Localizacao.Distance(pontoReferencia) * 111.32d
            })
            .Where(x => x.DistanciaKm <= raioKm)
            .OrderBy(x => x.DistanciaKm)
            .ToListAsync();

        return resultado
            .Select(x => (x.Estabelecimento, x.DistanciaKm))
            .ToList();
    }

    private static IQueryable<Estabelecimento> AplicarFiltros(IQueryable<Estabelecimento> query, string? nome, string? categoria)
    {
        if (!string.IsNullOrWhiteSpace(nome))
        {
            var nomeNormalizado = nome.Trim().ToLower();
            query = query.Where(e => e.Nome.ToLower().Contains(nomeNormalizado));
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            var categoriaNormalizada = categoria.Trim().ToLower();
            query = query.Where(e => e.Categoria.ToLower() == categoriaNormalizada);
        }

        return query;
    }
}
