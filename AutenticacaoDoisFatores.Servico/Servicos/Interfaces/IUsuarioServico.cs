using AutenticacaoDoisFatores.Servico.DTO.Usuario;

namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IUsuarioServico
    {
        Task<UsuarioResposta?> CadastrarAsync(UsuarioCadastrar usuarioCadastrar, string urlBase);
        Task<bool> AtivarAsync(string token);
        Task<UsuarioResposta?> AlterarNomeAsync(int id, UsuarioAlterarNome usuarioAlterarNome);
        Task<bool> EnviarEmailAlteracaoEmailAsync(int id, UsuarioAlterarEmail usuarioAlterarEmail, string urlBase);
        Task<UsuarioResposta?> AlterarEmailAsync(string token);
    }
}
