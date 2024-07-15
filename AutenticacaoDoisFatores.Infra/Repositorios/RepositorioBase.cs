using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Infra.Contexto;

namespace AutenticacaoDoisFatores.Infra.Repositorios
{
    public abstract class RepositorioBase(AutenticacaoDoisFatoresContexto contexto) : IRepositorioBase
    {
        protected readonly AutenticacaoDoisFatoresContexto _contexto = contexto;

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _contexto.SaveChangesAsync();
        }
    }
}
