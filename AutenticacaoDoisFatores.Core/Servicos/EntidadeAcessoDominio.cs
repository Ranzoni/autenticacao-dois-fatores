using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class EntidadeAcessoDominio(IEntidadeAcessoRepositorio repositorio) : IEntidadeAcessoDominio
    {
        private readonly IEntidadeAcessoRepositorio _repositorio = repositorio;

        public async Task AtivarEntidadeAcessoAsync(string email, bool ativar)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);
            if (entidadeAcesso is null)
            {
                EntidadeAcessoException.NaoEncontrada();
                return;
            }

            entidadeAcesso.Ativar(ativar);

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();
        }

        public async Task<EntidadeAcesso?> BuscarComEmailAsync(string email)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);

            return entidadeAcesso;
        }

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

        public async Task<EntidadeAcesso?> GerarNovaChaveAsync(string email)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);
            if (entidadeAcesso is null)
                return null;

            entidadeAcesso.GerarChave();

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }
    }
}
