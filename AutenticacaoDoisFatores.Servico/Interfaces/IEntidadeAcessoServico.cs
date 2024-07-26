using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;

namespace AutenticacaoDoisFatores.Servico.Interfaces
{
    public interface IEntidadeAcessoServico
    {
        Task<EntidadeAcessoResposta?> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar, string urlBase);
        Task<bool> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso, string urlBase);
        Task<bool> AlterarChaveAcessoAsync(string token);
        Task<EntidadeAcessoResposta?> AtivarCadastroAsync(string token);
        Task<EntidadeAcessoResposta?> AlterarNomeAsync(string token);
        Task<bool> EnviarEmailAlteracaoNomeAsync(EntidadeAcessoAlterar entidadeAcessoAlterar, string urlBase);
        Task<EntidadeAcessoResposta?> AlterarEmailAsync(string emailAtual, EntidadeAcessoAlterarEmail entidadeAcessoAlterarEmail);
    }
}
