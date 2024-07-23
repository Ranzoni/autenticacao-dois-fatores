using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;

namespace AutenticacaoDoisFatores.Servico.Interfaces
{
    public interface IEntidadeAcessoServico
    {
        Task<EntidadeAcessoCadastrada?> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar, string urlBase);
        Task ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso, string urlBase);
        Task AlterarChaveAcessoAsync(string token);
        Task AtivarCadastroAsync(string token);
    }
}
