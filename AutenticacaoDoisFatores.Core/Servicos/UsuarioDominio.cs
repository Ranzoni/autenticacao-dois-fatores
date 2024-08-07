using AutenticacaoDoisFatores.Core.Entidades;
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

        public async Task CadastrarAsync(Usuario usuario)
        {
            await _repositorio.CadastrarAsync(usuario);
            await _repositorio.SalvarAlteracoesAsync();
        }

        public async Task<bool> ExisteUsuarioComEmailAsync(string email)
        {
            return await _repositorio.ExisteUsuarioComEmailAsync(email);
        }
    }
}
