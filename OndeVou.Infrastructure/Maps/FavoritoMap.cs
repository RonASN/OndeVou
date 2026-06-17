using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OndeVou.Domain.Entities;

namespace OndeVou.Infrastructure.Maps;

public class FavoritoMap : IEntityTypeConfiguration<Favorito>
{
    public void Configure(EntityTypeBuilder<Favorito> entity)
    {
        entity.ToTable("favoritos");

        entity.HasKey(e => new { e.UsuarioId, e.EstabelecimentoId });

        entity.Property(e => e.DataCriacao)
            .IsRequired();

        entity.HasOne(e => e.Usuario)
            .WithMany(u => u.Favoritos)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Estabelecimento)
            .WithMany(e => e.Favoritos)
            .HasForeignKey(e => e.EstabelecimentoId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => e.EstabelecimentoId);
    }
}