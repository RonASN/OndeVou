using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OndeVou.Api.Helpers;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;

namespace OndeVou.Api.Controllers;

[ApiController]
[Route("api/avaliacoes")]
public class AvaliacaoController : ControllerBase
{
    private readonly IAvaliacaoService _avaliacaoService;

    public AvaliacaoController(IAvaliacaoService avaliacaoService)
    {
        _avaliacaoService = avaliacaoService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarAvaliacaoRequestDto request)
    {
        try
        {
            var usuarioId = ClaimsHelper.GetUsuarioId(User);
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var resultado = await _avaliacaoService.CriarAsync(request, usuarioId.Value);
            return Ok(resultado);
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

    [HttpGet("estabelecimento/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarPorEstabelecimento(int id)
    {
        try
        {
            var resultado = await _avaliacaoService.ListarPorEstabelecimentoIdAsync(id);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet("estabelecimento/{id}/resumo")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterResumo(int id)
    {
        try
        {
            var resultado = await _avaliacaoService.ObterResumoPorEstabelecimentoIdAsync(id);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}