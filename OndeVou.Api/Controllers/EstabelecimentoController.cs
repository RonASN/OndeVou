using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OndeVou.Application.DTOs.Request;
using OndeVou.Application.Interfaces;

namespace OndeVou.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            var resultado = await _estabelecimentoService.CriarAsync(request);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
