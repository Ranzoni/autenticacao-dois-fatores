using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Validadores
{
    public static class UsuarioValidador
    {
        internal static void ValidarNovoUsuario(string nome, string email, string senha, EntidadeAcesso entidadeAcesso)
        {
            if (!NomeEhValido(nome))
                UsuarioException.NomeInvalido();

            if (!EmailEhValido(email))
                UsuarioException.EmailInvalido();

            if (!SenhaEhValida(senha))
                UsuarioException.SenhaInvalida();

            if (entidadeAcesso is null)
                UsuarioException.EntidadeAcessoNaoInformada();
        }

        internal static void ValidarUsuarioCompleto(int id, string nome, string email, string senha)
        {
            if (id <= 0)
                UsuarioException.IdInvalido();

            if (!NomeEhValido(nome))
                UsuarioException.NomeInvalido();

            if (!EmailEhValido(email))
                UsuarioException.EmailInvalido();

            if (!SenhaEhValida(senha))
                UsuarioException.SenhaInvalida();
        }

        public static bool NomeEhValido(string nome)
        {
            return !nome.IsNullOrEmptyOrWhiteSpaces() && nome.Length >= 3 && nome.Length <= 50;
        }

        public static bool EmailEhValido(string email)
        {
            var regex = Utilitarios.RetornarEmailRegex();

            return !email.IsNullOrEmptyOrWhiteSpaces() && email.Length >= 5 && email.Length <= 80 && regex.IsMatch(email);
        }

        public static bool SenhaEhValida(string senha)
        {
            return !senha.IsNullOrEmptyOrWhiteSpaces();
        }
    }
}
