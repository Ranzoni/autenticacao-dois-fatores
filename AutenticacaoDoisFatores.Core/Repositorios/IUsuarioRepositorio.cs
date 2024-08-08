using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios.Base;

namespace AutenticacaoDoisFatores.Core.Repositorios
{
    public interface IUsuarioRepositorio : IRepositorioBase
    {
        Task CadastrarAsync(Usuario usuario);
        Task<bool> ExisteUsuarioComEmailAsync(string email);
        void Alterar(Usuario usuario);
        Task<Usuario?> BuscarAsync(int id);
    }
}
