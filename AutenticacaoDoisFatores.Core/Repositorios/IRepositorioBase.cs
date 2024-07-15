namespace AutenticacaoDoisFatores.Core.Repositorios
{
    public interface IRepositorioBase
    {
        Task<int> SalvarAlteracoesAsync();
    }
}
