using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios.Base;

namespace AutenticacaoDoisFatores.Core.Repositorios
{
    public interface IUsuarioRepositorio : IRepositorioBase
    {
        Task CadastrarAsync(Usuario usuario);
    }
}
