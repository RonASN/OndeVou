using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequestDto request)
    {
        try
        {
            var resultado = await _usuarioService.CriarAsync(request);
            return Ok(resultado);
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
}
