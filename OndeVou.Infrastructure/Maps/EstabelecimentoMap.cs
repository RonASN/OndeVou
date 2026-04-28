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
    }
}
