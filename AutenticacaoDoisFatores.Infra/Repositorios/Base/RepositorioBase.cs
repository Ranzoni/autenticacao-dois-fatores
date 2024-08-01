using AutenticacaoDoisFatores.Core.Repositorios.Base;
using AutenticacaoDoisFatores.Infra.Contexto;

namespace AutenticacaoDoisFatores.Infra.Repositorios.Base
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
