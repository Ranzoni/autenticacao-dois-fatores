using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;

namespace AutenticacaoDoisFatores.Servico.Excecoes
{
    internal class EntidadeAcessoServicoException(NotificacoesEntidadeAcesso message) : ApplicationException(message.Descricao())
    {
        internal static void FalhaAoRecuperarChaveAcesso()
        {
            throw new EntidadeAcessoServicoException(NotificacoesEntidadeAcesso.FalhaAoRecuperarChaveAcesso);
        }
    }
}
