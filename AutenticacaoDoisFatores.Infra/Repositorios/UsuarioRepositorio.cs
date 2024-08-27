using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoDoisFatores.Infra.Repositorios
{
    public class UsuarioRepositorio(AutenticacaoDoisFatoresContexto _contexto) : IUsuarioRepositorio
    {
        public void Alterar(Usuario usuario)
        {
            _contexto.Usuarios.Update(usuario);
        }

        public async Task<Usuario?> BuscarAsync(int id, Guid chave)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Id.Equals(id)
                    && u.Ativo
                    && u.EntidadeAcesso.Chave.Equals(chave));
        }

        public async Task<Usuario?> BuscarNaoAtivoAsync(int id, Guid chave)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Id.Equals(id)
                    && u.EntidadeAcesso.Chave.Equals(chave));
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email, Guid chave)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Email.Equals(email)
                    && u.Ativo
                    && u.EntidadeAcesso.Chave.Equals(chave));
        }

        public async Task CadastrarAsync(Usuario usuario)
        {
            await _contexto.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> ExisteUsuarioComEmailAsync(string email, Guid chave)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Email.Equals(email) && u.EntidadeAcesso.Chave.Equals(chave));
        }

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _contexto.SaveChangesAsync();
        }
    }
}
