using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    public abstract class BaseController(INotificador notificador) : ControllerBase
    {
        private readonly INotificador _notificador = notificador;

        public ActionResult<T?> CriadoComSucesso<T>(T? retorno)
        {
            if (_notificador.ExisteMensagem())
                return UnprocessableEntity(_notificador.Mensagens());

            return StatusCode(201, retorno);
        }

        protected ActionResult AlteradoComSucesso()
        {
            if (_notificador.ExisteMensagem())
                return UnprocessableEntity(_notificador.Mensagens());

            return base.Ok("Alteração realizada com sucesso");
        }
    }
}
