using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using Mensageiro;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public class AutenticacaoServicoValidacao(INotificador _notificador)
    {
        public bool AtivacaoEhValida(Usuario? usuario)
        {
            if (usuario is null)
            {
                _notificador.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado);
                return false;
            }

            if (usuario.Ativo)
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailJaCadastrado);
                return false;
            }

            return true;
        }

        public void UsuarioNaoEncontrado()
        {
            _notificador.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado);
        }

        public void NaoAutorizado()
        {
            _notificador.AddMensagemNaoAutorizado(NotificacoesUsuario.SenhaIncorreta);
        }
    }
}
