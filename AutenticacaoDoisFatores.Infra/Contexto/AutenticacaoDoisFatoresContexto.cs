using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Infra.Contexto.Configuracoes;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoDoisFatores.Infra.Contexto
{
    public class AutenticacaoDoisFatoresContexto(DbContextOptions options) : DbContext(options)
    {
        public DbSet<EntidadeAcesso> EntidadesAcesso { get; private set; }
        public DbSet<Usuario> Usuarios { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntidadeAcessoConfiguracao());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracao());
        }
    }
}
