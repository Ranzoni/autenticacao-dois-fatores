using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Excecoes
{
    public class UsuarioException(NotificacoesUsuario message) : ApplicationException(message.Descricao())
    {
        public static void IdInvalido()
        {
            throw new UsuarioException(NotificacoesUsuario.IdInvalido);
        }

        public static void NomeInvalido()
        {
            throw new UsuarioException(NotificacoesUsuario.NomeInvalido);
        }

        public static void EmailInvalido()
        {
            throw new UsuarioException(NotificacoesUsuario.EmailInvalido);
        }

        public static void SenhaInvalida()
        {
            throw new UsuarioException(NotificacoesUsuario.SenhaInvalida);
        }

        public static void EntidadeAcessoNaoInformada()
        {
            throw new UsuarioException(NotificacoesUsuario.ChaveAcessoNaoInformada);
        }

        internal static void UsuarioNaoEncontrado()
        {
            throw new UsuarioException(NotificacoesUsuario.NaoEncontrado);
        }
    }
}
