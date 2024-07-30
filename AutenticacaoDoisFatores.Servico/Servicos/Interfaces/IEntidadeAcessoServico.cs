using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;

namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IEntidadeAcessoServico
    {
        Task<EntidadeAcessoResposta?> CadastrarAsync(EntidadeAcessoCadastrar entidadeCadastrar, string urlBase);
        Task<EntidadeAcessoResposta?> AtivarCadastroAsync(string token);
        Task<bool> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso, string urlBase);
        Task<bool> AlterarChaveAcessoAsync(string token);
        Task<bool> EnviarEmailAlteracaoNomeAsync(EntidadeAcessoAlterarNome entidadeAlterarNome, string urlBase);
        Task<EntidadeAcessoResposta?> AlterarNomeAsync(string token);
        Task<bool> EnviarEmailAlteracaoEmailAsync(EntidadeAcessoAlterarEmail entidadeAlterarEmail, string urlBase);
        Task<EntidadeAcessoResposta?> AlterarEmailAsync(string token);
        Task<bool> ExcluirAsync(EntidadeAcessoExcluir entidadeAcessoExcluir);
    }
}
