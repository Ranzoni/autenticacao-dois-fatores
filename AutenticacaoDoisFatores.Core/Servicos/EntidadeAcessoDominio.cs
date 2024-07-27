using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class EntidadeAcessoDominio(IEntidadeAcessoRepositorio _repositorio) : IEntidadeAcessoDominio
    {
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

        public async Task<EntidadeAcesso?> BuscarAsync(int id)
        {
            return await _repositorio.BuscarAsync(id);
        }

        public async Task<EntidadeAcesso?> BuscarComEmailAsync(string email)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);

            return entidadeAcesso;
        }

        public async Task<EntidadeAcesso?> AlterarAsync(EntidadeAcesso entidadeAcesso)
        {
            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }
    }
}
