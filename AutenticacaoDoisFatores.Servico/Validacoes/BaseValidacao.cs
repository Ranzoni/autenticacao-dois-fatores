using AutenticacaoDoisFatores.Servico.Interfaces;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public abstract class BaseValidacao(INotificador notificador)
    {
        protected readonly INotificador _notificador = notificador;
    }
}
