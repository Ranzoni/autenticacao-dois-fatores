using AutenticacaoDoisFatores.Core.Extensoes;
using System.Security;

namespace AutenticacaoDoisFatores.Servico.Utilitarios
{
    internal static class ChaveAcesso
    {
        internal static Guid GerarChave()
        {
            var chave = Guid.NewGuid();

            return chave;
        }

        internal static string RetornarChaveAdmin()
        {
            var chaveAdminEnv = Environment.GetEnvironmentVariable("CHAVE_ADMIN") ?? "";
            if (chaveAdminEnv.IsNullOrEmptyOrWhiteSpaces())
                throw new SecurityException("A chave de administrador não foi encontrada");

            return chaveAdminEnv;
        }
    }
}
