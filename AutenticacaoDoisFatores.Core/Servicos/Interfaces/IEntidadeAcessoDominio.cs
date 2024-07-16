using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Servicos.Interfaces
{
    public interface IEntidadeAcessoDominio
    {
        Task<EntidadeAcesso> CadastrarAsync(EntidadeAcesso entidadeAcesso);
    }
}
