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
}
