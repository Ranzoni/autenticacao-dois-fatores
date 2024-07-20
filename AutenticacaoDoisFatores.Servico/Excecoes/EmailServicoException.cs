using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Enum;

namespace AutenticacaoDoisFatores.Servico.Excecoes
{
    public class EmailServicoException(NotificacoesEmail message) : ApplicationException(message.Descricao())
    {
        public static void HostNaoEncontrado()
        {
            throw new EmailServicoException(NotificacoesEmail.HostNaoEncontrado);
        }

        public static void PortaNaoEncontrada()
        {
            throw new EmailServicoException(NotificacoesEmail.PortaNaoEncontrada);
        }

        public static void EmailRemetenteNaoEncontrado()
        {
            throw new EmailServicoException(NotificacoesEmail.EmailRemetenteNaoEncontrado);
        }

        public static void SenhaEmailNaoEncontrada()
        {
            throw new EmailServicoException(NotificacoesEmail.SenhaEmailNaoEncontrada);
        }
    }
}
