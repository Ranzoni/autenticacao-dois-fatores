using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IUsuarioDominio
    {
        Task CadastrarAsync(Usuario usuario);
    }
}
