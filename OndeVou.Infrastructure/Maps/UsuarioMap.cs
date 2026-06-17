using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OndeVou.Domain.Entities;
using OndeVou.Domain.Enums;

namespace OndeVou.Infrastructure.Maps;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> entity)
    {
        entity.ToTable("usuarios");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        entity.HasIndex(e => e.Email)
            .IsUnique();

        entity.Property(e => e.SenhaHash)
            .IsRequired();

        entity.Property(e => e.TipoUsuario)
            .IsRequired()
            .HasConversion<int>();

        entity.Property(e => e.DataCriacao)
            .IsRequired();

        // Relacionamento: Um usuário pode ter vários estabelecimentos
        entity.HasMany(u => u.Estabelecimentos)
            .WithOne(e => e.Usuario)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento: Um usuário pode ter vários favoritos
        entity.HasMany(u => u.Favoritos)
            .WithOne(f => f.Usuario)
            .HasForeignKey(f => f.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento: Um usuário pode ter várias avaliações
        entity.HasMany(u => u.Avaliacoes)
            .WithOne(a => a.Usuario)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
