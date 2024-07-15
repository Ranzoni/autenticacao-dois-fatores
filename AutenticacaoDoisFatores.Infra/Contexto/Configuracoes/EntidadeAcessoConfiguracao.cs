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
                .HasIndex(p => p.Chave);
        }
    }
}
