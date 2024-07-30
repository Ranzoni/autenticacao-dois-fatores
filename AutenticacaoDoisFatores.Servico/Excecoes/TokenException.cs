using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Enum;

namespace AutenticacaoDoisFatores.Servico.Excecoes
{
    public class TokenException(NotificacoesToken message) : ApplicationException(message.Descricao())
    {
        public static void ChaveNaoEncontrada()
        {
            throw new TokenException(NotificacoesToken.ChaveNaoEncontrada);
        }
    }
}
