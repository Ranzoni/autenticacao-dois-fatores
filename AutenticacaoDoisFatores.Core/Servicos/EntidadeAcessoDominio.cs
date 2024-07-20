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

        public async Task<bool> ExisteEntidadeComEmailAsync(string email)
        {
            var existe = await _repositorio.ExisteEntidadeComEmailAsync(email);

            return existe;
        }

        public async Task<EntidadeAcesso?> GerarNovaChaveAsync(int id)
        {
            var entidadeAcesso = await _repositorio.BuscarAsync(id);
            if (entidadeAcesso is null)
                return null;

            entidadeAcesso.GerarChave();

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }
    }
}
