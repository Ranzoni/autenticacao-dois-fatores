using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios.Base;

namespace AutenticacaoDoisFatores.Core.Repositorios
{
    public interface IUsuarioRepositorio : IRepositorioBase
    {
        Task CadastrarAsync(Usuario usuario);
        Task<bool> ExisteUsuarioComEmailAsync(string email, Guid chave);
        void Alterar(Usuario usuario);
        Task<Usuario?> BuscarAsync(int id, Guid chave);
        Task<Usuario?> BuscarNaoAtivoAsync(int id, Guid chave);
        Task<Usuario?> BuscarPorEmailAsync(string email, Guid chave);
    }
}
