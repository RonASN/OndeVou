using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OndeVou.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> CriarAsync(CriarUsuarioRequestDto request)
    {
        var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(request.Email);
        if (usuarioExistente != null)
        {
            throw new InvalidOperationException("Email já cadastrado");
        }

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = GerarHashSenha(request.Senha),
            DataCriacao = DateTime.UtcNow
        };

        var resultado = await _usuarioRepository.CriarAsync(usuario);

        return new UsuarioResponseDto
        {
            Id = resultado.Id,
            Nome = resultado.Nome,
            Email = resultado.Email,
            DataCriacao = resultado.DataCriacao
        };
    }

    private string GerarHashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
