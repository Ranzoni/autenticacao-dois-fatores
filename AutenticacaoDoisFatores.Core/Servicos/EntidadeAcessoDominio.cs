using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class EntidadeAcessoDominio(IEntidadeAcessoRepositorio repositorio) : IEntidadeAcessoDominio
    {
        private readonly IEntidadeAcessoRepositorio _repositorio = repositorio;

        public async Task<EntidadeAcesso> CadastrarAsync(EntidadeAcesso entidadeAcesso)
        {
            await _repositorio.CadastrarAsync(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }
    }
}
