using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Validadores
{
    internal static class EntidadeAcessoValidador
    {
        internal static void ValidarNovaEntidade(string nome, string email)
        {
            if (nome.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.NomeInvalido();

            if (email.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.EmailInvalido();
        }

        internal static void ValidarEntidadeCompleta(int id, string nome, string chave, string email)
        {
            if (id == 0)
                EntidadeAcessoException.IdInvalido();

            if (nome.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.NomeInvalido();

            if (chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.ChaveInvalida();

            if (email.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.EmailInvalido();
        }
    }
}
