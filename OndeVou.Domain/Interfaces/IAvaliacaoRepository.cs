using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IAvaliacaoRepository
{
    Task<bool> ExisteAsync(int usuarioId, int estabelecimentoId);
    Task<Avaliacao> CriarAsync(Avaliacao avaliacao);
    Task<List<Avaliacao>> ListarPorEstabelecimentoIdAsync(int estabelecimentoId);
    Task<(double Media, int Quantidade)> ObterResumoPorEstabelecimentoIdAsync(int estabelecimentoId);
}