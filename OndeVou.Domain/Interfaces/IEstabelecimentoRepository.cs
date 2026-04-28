using OndeVou.Domain.Entities;

namespace OndeVou.Domain.Interfaces;

public interface IEstabelecimentoRepository
{
    Task<Estabelecimento> CriarAsync(Estabelecimento estabelecimento);
}
