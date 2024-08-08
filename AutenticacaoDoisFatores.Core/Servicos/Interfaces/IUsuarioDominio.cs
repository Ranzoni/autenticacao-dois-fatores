using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IUsuarioDominio
    {
        Task CadastrarAsync(Usuario usuario);
        Task<bool> ExisteUsuarioComEmailAsync(string email);
        Task AlterarAsync(Usuario usuario);
        Task<Usuario?> BuscarAsync(int id);
    }
}
