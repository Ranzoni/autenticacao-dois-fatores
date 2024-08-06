namespace AutenticacaoDoisFatores.Servico.Utilitarios
{
    internal static class ChaveAcesso
    {
        internal static Guid GerarChave()
        {
            var chave = Guid.NewGuid();

            return chave;
        }
    }
}
