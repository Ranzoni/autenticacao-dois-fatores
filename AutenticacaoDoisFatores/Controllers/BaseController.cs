using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        protected ActionResult Sucesso(string mensagem)
        {
            if (_notificador.ExisteMensagem())
                return UnprocessableEntity(_notificador.Mensagens());

            return Ok(mensagem);
        }

        protected ActionResult Sucesso(object modelo)
        {
            if (_notificador.ExisteMensagem())
                return UnprocessableEntity(_notificador.Mensagens());

            return Ok(modelo);
        }

        protected ActionResult NaoEncontrado(string mensagem)
        {
            if (_notificador.ExisteMensagem())
                return UnprocessableEntity(_notificador.Mensagens());

            return NotFound(mensagem);
        }

        protected ContentResult MensagemHtml(string cabecalho, string titulo, string mensagem)
        {
            var httpStatusCode = HttpStatusCode.OK;
            if (_notificador.ExisteMensagem())
            {
                httpStatusCode = HttpStatusCode.UnprocessableEntity;
                cabecalho = "Atenção";
                titulo = "Operação não realizada";
                mensagem = _notificador.Mensagens().First();
            }

            var style = @"
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f8f9fa;
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                height: 100vh;
                                margin: 0;
                            }

                            .container {
                                text-align: center;
                                padding: 20px;
                                border: 1px solid #e0e0e0;
                                border-radius: 8px;
                                background-color: #ffffff;
                                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                            }

                            .message-box h1 {
                                color: #4285f4;
                                margin-bottom: 10px;
                            }

                            .message-box p {
                                color: #555555;
                            }
                        </style>";

            var html = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='utf-8' />
                            <title>{cabecalho}</title>
                            {style}
                        </head>
                        <body>
                            <div class='container'>
                                <div class='message-box'>
                                    <h1>{titulo}</h1>
                                    <p>{mensagem}</p>
                                </div>
                            </div>
                        </body>
                        </html>";

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)httpStatusCode,
                Content = html
            };
        }
    }
}
