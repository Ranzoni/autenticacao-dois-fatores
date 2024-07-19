using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Validadores
{
    internal static class EntidadeAcessoValidador
    {
        internal static void ValidarNovaEntidade(string nome, string email)
        {
            if (nome.IsNullOrEmptyOrWhiteSpaces() || nome.Length < 3 || nome.Length > 50)
                EntidadeAcessoException.NomeInvalido();

            if (email.IsNullOrEmptyOrWhiteSpaces() || email.Length < 5 || email.Length > 80)
                EntidadeAcessoException.EmailInvalido();
        }

        internal static void ValidarEntidadeCompleta(int id, string nome, string chave, string email)
        {
            if (id == 0)
                EntidadeAcessoException.IdInvalido();

            if (nome.IsNullOrEmptyOrWhiteSpaces() || nome.Length < 3 || nome.Length > 50)
                EntidadeAcessoException.NomeInvalido();

            if (chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.ChaveInvalida();

            if (email.IsNullOrEmptyOrWhiteSpaces() || email.Length < 5 || email.Length > 80)
                EntidadeAcessoException.EmailInvalido();
        }
    }
}
