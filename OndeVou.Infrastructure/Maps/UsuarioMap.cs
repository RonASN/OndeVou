using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OndeVou.Domain.Entities;

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

        entity.Property(e => e.DataCriacao)
            .IsRequired();
    }
}
