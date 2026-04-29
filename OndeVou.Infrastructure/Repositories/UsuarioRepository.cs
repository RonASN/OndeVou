using Microsoft.EntityFrameworkCore;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Interfaces;
using OndeVou.Infrastructure.Data;

namespace OndeVou.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> CriarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
