using OndeVou.Domain.Entities;

namespace OndeVou.Application.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
