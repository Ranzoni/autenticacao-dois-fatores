using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Core.Excecoes
{
    public class CriptografiaException(NotificacoesCriptografia message) : ApplicationException(message.Descricao())
    {
        public static void ChaveNaoEncontrada()
        {
            throw new CriptografiaException(NotificacoesCriptografia.ChaveNaoEncontrada);
        }
    }
}
