using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using System.Text.RegularExpressions;

namespace AutenticacaoDoisFatores.Core.Validadores
{
    public static partial class EntidadeAcessoValidador
    {
        internal static void ValidarNovaEntidade(string nome, string chave, string email)
        {
            if (!NomeEhValido(nome))
                EntidadeAcessoException.NomeInvalido();

            if (chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.ChaveInvalida();

            if (!EmailEhValido(email))
                EntidadeAcessoException.EmailInvalido();
        }

        internal static void ValidarEntidadeCompleta(int id, string nome, string chave, string email)
        {
            if (id == 0)
                EntidadeAcessoException.IdInvalido();

            if (!NomeEhValido(nome))
                EntidadeAcessoException.NomeInvalido();

            if (chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoException.ChaveInvalida();

            if (!EmailEhValido(email))
                EntidadeAcessoException.EmailInvalido();
        }

        public static bool NomeEhValido(string nome)
        {
            return !nome.IsNullOrEmptyOrWhiteSpaces() && nome.Length >= 3 && nome.Length <= 50;
        }

        public static bool EmailEhValido(string email)
        {
            var regex = EmailRegex();

            return !email.IsNullOrEmptyOrWhiteSpaces() && email.Length >= 5 && email.Length <= 80 && regex.IsMatch(email);
        }

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
