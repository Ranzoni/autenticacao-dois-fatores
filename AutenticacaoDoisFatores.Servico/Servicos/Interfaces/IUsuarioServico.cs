using AutenticacaoDoisFatores.Servico.DTO.Usuario;

namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IUsuarioServico
    {
        Task<UsuarioResposta?> CadastrarAsync(UsuarioCadastrar usuarioCadastrar, string urlBase);
        Task<bool> AtivarAsync(string token);
    }
}
