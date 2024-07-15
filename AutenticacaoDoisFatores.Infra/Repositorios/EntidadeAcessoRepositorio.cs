using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Infra.Contexto;

namespace AutenticacaoDoisFatores.Infra.Repositorios
{
    public class EntidadeAcessoRepositorio(AutenticacaoDoisFatoresContexto contexto) : RepositorioBase(contexto), IEntidadeAcessoRepositorio
    {
        public async Task CadastrarAsync(EntidadeAcesso entidadeAcesso)
        {
            await _contexto.AddAsync(entidadeAcesso);
        }
    }
}
