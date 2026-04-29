using System.Security.Claims;

namespace OndeVou.Api.Helpers;

public static class ClaimsHelper
{
    public static int? GetUsuarioId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        return null;
    }

    public static string? GetUsuarioEmail(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static string? GetUsuarioNome(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static int? GetTipoUsuario(ClaimsPrincipal user)
    {
        var tipoClaim = user.FindFirst("tipo")?.Value;
        if (int.TryParse(tipoClaim, out var tipo))
        {
            return tipo;
        }
        return null;
    }
}
