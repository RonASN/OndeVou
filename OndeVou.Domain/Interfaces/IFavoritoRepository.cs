using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IFavoritoRepository
{
    Task<bool> ExisteAsync(int usuarioId, int estabelecimentoId);
    Task<Favorito> CriarAsync(Favorito favorito);
    Task<bool> RemoverAsync(int usuarioId, int estabelecimentoId);
    Task<List<Favorito>> ListarPorUsuarioIdAsync(int usuarioId);
}