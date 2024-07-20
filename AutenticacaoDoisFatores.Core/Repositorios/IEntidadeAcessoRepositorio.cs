using AutenticacaoDoisFatores.Core.Entidades;

namespace AutenticacaoDoisFatores.Core.Repositorios
{
    public interface IEntidadeAcessoRepositorio : IRepositorioBase
    {
        Task CadastrarAsync(EntidadeAcesso entidadeAcesso);
        Task<bool> ExisteEntidadeComEmailAsync(string email);
        Task<EntidadeAcesso?> BuscarAsync(int id);
        void Alterar(EntidadeAcesso entidadeAcesso);
    }
}
