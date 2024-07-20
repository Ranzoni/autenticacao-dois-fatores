using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Enum;

namespace AutenticacaoDoisFatores.Servico.Excecoes
{
    internal class EntidadeAcessoServicoException(NotificacoesEntidadeAcessoServico message) : ApplicationException(message.Descricao())
    {
        internal static void FalhaAoRecuperarChaveAcesso()
        {
            throw new EntidadeAcessoServicoException(NotificacoesEntidadeAcessoServico.FalhaAoRecuperarChaveAcesso);
        }
    }
}
