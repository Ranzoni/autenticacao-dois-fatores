using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IEntidadeAcessoDominio
    {
        Task<EntidadeAcesso> CadastrarAsync(EntidadeAcesso entidadeAcesso);
        Task<bool> ExisteEntidadeComEmailAsync(string email);
        Task<EntidadeAcesso?> BuscarAsync(int id);
        Task<EntidadeAcesso?> BuscarComEmailAsync(string email);
        Task<EntidadeAcesso?> AlterarAsync(EntidadeAcesso entidadeAcesso);
        Task<bool> ExcluirAsync(int id);
        Task<EntidadeAcesso?> BuscarComChaveAsync(string chave);
    }
}
