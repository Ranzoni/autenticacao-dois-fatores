using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Excecoes
{
    public class EntidadeAcessoException(NotificacoesEntidadeAcesso mensagem) : ApplicationException(mensagem.Descricao())
    {
        internal static void IdInvalido()
        {
            throw new EntidadeAcessoException(NotificacoesEntidadeAcesso.IdInvalido);
        }

        internal static void NomeInvalido()
        {
            throw new EntidadeAcessoException(NotificacoesEntidadeAcesso.NomeInvalido);
        }

        internal static void EmailInvalido()
        {
            throw new EntidadeAcessoException(NotificacoesEntidadeAcesso.EmailInvalido);
        }
    }
}
