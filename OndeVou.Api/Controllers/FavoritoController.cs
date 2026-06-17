using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OndeVou.Api.Helpers;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.Exceptions;
using OndeVou.Application.Interfaces;

namespace OndeVou.Api.Controllers;

[ApiController]
[Route("api/favoritos")]
[Authorize]
public class FavoritoController : ControllerBase
{
    private readonly IFavoritoService _favoritoService;

    public FavoritoController(IFavoritoService favoritoService)
    {
        _favoritoService = favoritoService;
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarFavoritoRequestDto request)
    {
        try
        {
            var usuarioId = ClaimsHelper.GetUsuarioId(User);
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            await _favoritoService.AdicionarAsync(request, usuarioId.Value);
            return Ok(new { mensagem = "Favorito adicionado com sucesso" });
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

    [HttpDelete("{estabelecimentoId}")]
    public async Task<IActionResult> Remover(int estabelecimentoId)
    {
        try
        {
            var usuarioId = ClaimsHelper.GetUsuarioId(User);
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var removido = await _favoritoService.RemoverAsync(usuarioId.Value, estabelecimentoId);
            if (!removido)
            {
                return NotFound(new { mensagem = "Favorito não encontrado" });
            }

            return Ok(new { mensagem = "Favorito removido com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor", detalhes = ex.Message });
        }
    }

    [HttpGet("meus")]
    public async Task<IActionResult> ListarMeus()
    {
        try
        {
            var usuarioId = ClaimsHelper.GetUsuarioId(User);
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var resultado = await _favoritoService.ListarMeusAsync(usuarioId.Value);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor", detalhes = ex.Message });
        }
    }
}