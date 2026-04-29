using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.Interfaces;

namespace OndeVou.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequestDto request)
    {
        try
        {
            var resultado = await _usuarioService.CriarAsync(request);
            return CreatedAtAction(nameof(Criar), new { id = resultado.Id }, resultado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var resultado = await _usuarioService.LoginAsync(request);
            return Ok(resultado);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
