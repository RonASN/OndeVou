using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IEstabelecimentoRepository
{
    Task<Estabelecimento> CriarAsync(Estabelecimento estabelecimento);
    Task<List<Estabelecimento>> ListarAsync(string? nome, string? categoria, int skip, int take);
    Task<int> ContarAsync(string? nome, string? categoria);
    Task<List<Estabelecimento>> ListarOrdenadoAsync(string? nome, string? categoria, int page, int pageSize, string? ordenarPor);
    Task<Estabelecimento?> BuscarPorIdAsync(int id);
    Task<List<Estabelecimento>> ListarTodosAsync();
    Task<List<Estabelecimento>> ListarTodosFiltradosAsync(string? nome, string? categoria);
    Task<List<Estabelecimento>> ListarPorUsuarioIdAsync(int usuarioId);
    Task<List<(Estabelecimento Estabelecimento, double DistanciaKm)>> ListarProximosAsync(double latitude, double longitude, double raioKm);
}
