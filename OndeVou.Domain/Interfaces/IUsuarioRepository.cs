using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> CriarAsync(Usuario usuario);
    Task<Usuario?> BuscarPorEmailAsync(string email);
}
