using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;

namespace AutenticacaoDoisFatores.Servico.Interfaces
{
    public interface IEntidadeAcessoServico
    {
        Task<EntidadeAcessoCadastrada?> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar, string urlBase);
        Task ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso, string urlBase);
        Task AlterarChaveAcessoAsync(string token);
        Task AtivarCadastroAsync(string token);
        Task AlterarEntidadeAcessoAsync(string token);
        Task EnviarEmailAlteracaoEntidadeAsync(EntidadeAcessoAlterar entidadeAcessoAlterar, string urlBase);
    }
}
