using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IEntidadeAcessoDominio
    {
        Task<EntidadeAcesso> CadastrarAsync(EntidadeAcesso entidadeAcesso);
        Task<bool> ExisteEntidadeComEmailAsync(string email);
        Task<EntidadeAcesso?> GerarNovaChaveAsync(string email);
        Task AtivarEntidadeAcessoAsync(string email, bool ativar);
        Task<EntidadeAcesso?> BuscarComEmailAsync(string email);
    }
}
