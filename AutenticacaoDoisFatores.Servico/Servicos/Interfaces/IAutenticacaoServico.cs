using AutenticacaoDoisFatores.Servico.DTO.Autenticacao;

namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IAutenticacaoServico
    {
        Task<UsuarioAutenticado?> AutenticarAsync(UsuarioAutenticar usuarioAutenticar);
        Task<bool> AtivarAsync(string token);
        Task<bool> InativarAsync(int id, Guid chave);
    }
}
