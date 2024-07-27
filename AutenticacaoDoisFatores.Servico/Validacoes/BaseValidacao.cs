using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public abstract class BaseValidacao(INotificadorServico notificador)
    {
        protected readonly INotificadorServico _notificador = notificador;
    }
}
