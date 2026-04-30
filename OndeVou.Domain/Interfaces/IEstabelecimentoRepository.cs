using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IEstabelecimentoRepository
{
    Task<Estabelecimento> CriarAsync(Estabelecimento estabelecimento);
    Task<List<Estabelecimento>> ListarAsync(string? nome, string? categoria, int skip, int take);
    Task<Estabelecimento?> BuscarPorIdAsync(int id);
    Task<List<Estabelecimento>> ListarTodosAsync();
    Task<List<Estabelecimento>> ListarPorUsuarioIdAsync(int usuarioId);
}
