using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OndeVou.Domain.Entities;

namespace OndeVou.Infrastructure.Maps;

public class EstabelecimentoMap : IEntityTypeConfiguration<Estabelecimento>
{
    public void Configure(EntityTypeBuilder<Estabelecimento> entity)
    {
        entity.ToTable("estabelecimentos");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(e => e.Categoria)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Localizacao)
            .IsRequired()
            .HasColumnType("geometry (Point, 4326)");

        entity.Property(e => e.DataCriacao)
            .IsRequired();

        entity.Property(e => e.UsuarioId)
            .IsRequired();

        entity.HasOne(e => e.Usuario)
            .WithMany(u => u.Estabelecimentos)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => e.Nome);
        entity.HasIndex(e => e.Categoria);
        entity.HasIndex(e => e.DataCriacao);
        entity.HasIndex(e => e.Localizacao)
            .HasMethod("GIST");
    }
}
