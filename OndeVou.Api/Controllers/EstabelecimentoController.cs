using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OndeVou.Api.Helpers;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;

namespace OndeVou.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstabelecimentoController : ControllerBase
{
    private readonly IEstabelecimentoService _estabelecimentoService;

    public EstabelecimentoController(IEstabelecimentoService estabelecimentoService)
    {
        _estabelecimentoService = estabelecimentoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarEstabelecimentoRequestDto request)
    {
        try
        {
            var usuarioId = ClaimsHelper.GetUsuarioId(User);

            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var resultado = await _estabelecimentoService.CriarAsync(request, usuarioId.Value);
            return CreatedAtAction(nameof(Criar), new { id = resultado.Id }, resultado);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor", detalhes = ex.Message });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar([FromQuery] EstabelecimentoFiltroRequestDto filtro)
    {
        try
        {
            var resultado = await _estabelecimentoService.ListarAsync(filtro);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var resultado = await _estabelecimentoService.ObterPorIdAsync(id);

            if (resultado == null)
                return NotFound(new { mensagem = "Estabelecimento não encontrado" });

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("geo")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarGeoJson()
    {
        try
        {
            var resultado = await _estabelecimentoService.ListarGeoJsonAsync();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("usuario/{usuarioId}")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarPorUsuarioId(int usuarioId)
    {
        try
        {
            var resultado = await _estabelecimentoService.ListarPorUsuarioIdAsync(usuarioId);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
