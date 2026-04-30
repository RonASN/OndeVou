using OndeVou.Application.DTOs.Request;
using OndeVou.Application.DTOs.Response;
using OndeVou.Application.Interfaces;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Enums;
using OndeVou.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace OndeVou.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<UsuarioResponseDto> CriarAsync(CriarUsuarioRequestDto request)
    {
        // Normalizar email
        var emailNormalizado = request.Email.Trim().ToLower();

        var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(emailNormalizado);
        if (usuarioExistente != null)
        {
            throw new InvalidOperationException("Email já cadastrado");
        }

        // Validar TipoUsuario
        if (!Enum.IsDefined(typeof(TipoUsuario), request.TipoUsuario))
        {
            throw new ArgumentException("Tipo de usuário inválido");
        }

        var usuario = new Usuario
        {
            Nome = request.Nome.Trim(),
            Email = emailNormalizado,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            TipoUsuario = (TipoUsuario)request.TipoUsuario,
            DataCriacao = DateTime.UtcNow
        };

        var resultado = await _usuarioRepository.CriarAsync(usuario);

        return new UsuarioResponseDto
        {
            Id = resultado.Id,
            Nome = resultado.Nome,
            Email = resultado.Email,
            TipoUsuario = (int)resultado.TipoUsuario,
            TipoUsuarioDescricao = resultado.TipoUsuario.ToString(),
            DataCriacao = resultado.DataCriacao
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Normalizar email
        var emailNormalizado = request.Email.Trim().ToLower();

        var usuario = await _usuarioRepository.BuscarPorEmailAsync(emailNormalizado);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        var token = _tokenService.GerarToken(usuario);
        var expireHours = int.Parse(_configuration["Jwt:ExpireHours"]!);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(expireHours)
        };
    }

    public async Task<List<UsuarioResponseDto>> ListarAsync(PaginacaoRequestDto paginacao)
    {
        var usuarios = await _usuarioRepository.ListarAsync(paginacao.Skip, paginacao.Take);

        return usuarios.Select(u => new UsuarioResponseDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            TipoUsuario = (int)u.TipoUsuario,
            TipoUsuarioDescricao = u.TipoUsuario.ToString(),
            DataCriacao = u.DataCriacao
        }).ToList();
    }

    public async Task<UsuarioResponseDto?> ObterPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.BuscarPorIdAsync(id);

        if (usuario == null)
            return null;

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = (int)usuario.TipoUsuario,
            TipoUsuarioDescricao = usuario.TipoUsuario.ToString(),
            DataCriacao = usuario.DataCriacao
        };
    }
}
