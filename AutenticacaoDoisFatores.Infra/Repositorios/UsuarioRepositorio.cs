using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoDoisFatores.Infra.Repositorios
{
    public class UsuarioRepositorio(AutenticacaoDoisFatoresContexto _contexto) : IUsuarioRepositorio
    {
        public async Task CadastrarAsync(Usuario usuario)
        {
            await _contexto.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> ExisteUsuarioComEmailAsync(string email)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Email.Equals(email));
        }

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _contexto.SaveChangesAsync();
        }
    }
}
