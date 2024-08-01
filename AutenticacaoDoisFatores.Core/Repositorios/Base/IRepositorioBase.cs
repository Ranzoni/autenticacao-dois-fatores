namespace AutenticacaoDoisFatores.Core.Repositorios.Base
{
    public interface IRepositorioBase
    {
        Task<int> SalvarAlteracoesAsync();
    }
}
