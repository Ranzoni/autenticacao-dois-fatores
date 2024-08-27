using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IUsuarioDominio
    {
        Task CadastrarAsync(Usuario usuario);
        Task<bool> ExisteUsuarioComEmailAsync(string email, Guid chave);
        Task AlterarAsync(Usuario usuario);
        Task<Usuario?> BuscarAsync(int id, Guid chave);
        Task<Usuario?> BuscarNaoAtivoAsync(int id, Guid chave);
        Task<Usuario?> BuscarPorEmailAsync(string email, Guid chave);
    }
}
