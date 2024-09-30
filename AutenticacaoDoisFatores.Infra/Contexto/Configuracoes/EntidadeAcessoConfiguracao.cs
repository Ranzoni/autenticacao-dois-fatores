using AutenticacaoDoisFatores.Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutenticacaoDoisFatores.Infra.Contexto.Configuracoes
{
    public class EntidadeAcessoConfiguracao : IEntityTypeConfiguration<EntidadeAcesso>
    {
        public void Configure(EntityTypeBuilder<EntidadeAcesso> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(p => p.Chave)
                .IsRequired();

            builder
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(80);

            builder
                .HasIndex(p => p.Chave)
                .IsUnique();

            builder
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}
