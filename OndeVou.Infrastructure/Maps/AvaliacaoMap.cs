using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OndeVou.Domain.Entities;

namespace OndeVou.Infrastructure.Maps;

public class AvaliacaoMap : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> entity)
    {
        entity.ToTable("avaliacoes");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Nota)
            .IsRequired();

        entity.Property(e => e.Comentario)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(e => e.DataCriacao)
            .IsRequired();

        entity.Property(e => e.UsuarioId)
            .IsRequired();

        entity.Property(e => e.EstabelecimentoId)
            .IsRequired();

        entity.HasOne(e => e.Usuario)
            .WithMany(u => u.Avaliacoes)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Estabelecimento)
            .WithMany(e => e.Avaliacoes)
            .HasForeignKey(e => e.EstabelecimentoId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.UsuarioId, e.EstabelecimentoId })
            .IsUnique();

        entity.HasIndex(e => e.EstabelecimentoId);
    }
}