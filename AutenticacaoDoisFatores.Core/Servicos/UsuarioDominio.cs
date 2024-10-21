using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class UsuarioDominio(IUsuarioRepositorio _repositorio) : IUsuarioDominio
    {
        public async Task AlterarAsync(Usuario usuario)
        {
            _repositorio.Alterar(usuario);
            await _repositorio.SalvarAlteracoesAsync();
        }

        public async Task<Usuario?> BuscarAsync(int id, Guid chave)
        {
            return await _repositorio.BuscarAsync(id, chave);
        }

        public async Task<Usuario?> BuscarAtivoAsync(int id, Guid chave)
        {
            return await _repositorio.BuscarAtivoAsync(id, chave);
        }

        public async Task<Usuario?> BuscarNaoAtivoAsync(int id, Guid chave)
        {
            return await _repositorio.BuscarNaoAtivoAsync(id, chave);
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email, Guid chave)
        {
            return await _repositorio.BuscarPorEmailAsync(email, chave);
        }

        public async Task CadastrarAsync(Usuario usuario)
        {
            await _repositorio.CadastrarAsync(usuario);
            await _repositorio.SalvarAlteracoesAsync();
        }

        public async Task ExcluirAsync(int id, Guid chave)
        {
            var usuario = await BuscarAsync(id, chave);
            if (usuario is null)
            {
                UsuarioException.UsuarioNaoEncontrado();
                return;
            }

            _repositorio.Excluir(usuario);
            await _repositorio.SalvarAlteracoesAsync();
        }

        public async Task<bool> ExisteUsuarioComEmailAsync(string email, Guid chave)
        {
            return await _repositorio.ExisteUsuarioComEmailAsync(email, chave);
        }
    }
}
