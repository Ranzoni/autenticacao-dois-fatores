using AutenticacaoDoisFatores.Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutenticacaoDoisFatores.Infra.Contexto.Configuracoes
{
    public class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(80);

            builder
                .HasIndex(p => p.Email)
                .IsUnique();

            builder
                .Property(p => p.Senha)
                .IsRequired();

            builder
                .HasOne(p => p.EntidadeAcesso)
                .WithMany()
                .HasForeignKey(p => p.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
