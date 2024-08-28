using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Core.Validadores;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using Mensageiro;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public class UsuarioServicoValidacao(INotificador _notificador, IUsuarioDominio _dominio)
    {
        public async Task<bool> CadastroEhValidoAsync(UsuarioCadastrar usuarioCadastrar)
        {
            if (!UsuarioValidador.NomeEhValido(usuarioCadastrar.Nome))
            {
                _notificador.AddMensagem(NotificacoesUsuario.NomeInvalido);
                return false;
            }

            if (!UsuarioValidador.EmailEhValido(usuarioCadastrar.Email))
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailInvalido);
                return false;
            }

            if (!UsuarioValidador.SenhaEhValida(usuarioCadastrar.Senha))
            {
                _notificador.AddMensagem(NotificacoesUsuario.SenhaInvalida);
                return false;
            }

            var emailJaCadastrado = await _dominio.ExisteUsuarioComEmailAsync(usuarioCadastrar.Email, usuarioCadastrar.Chave);
            if (emailJaCadastrado)
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailJaCadastrado);
                return false;
            }

            return true;
        }

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

        public void ChaveAcessoNaoEncontrado()
        {
            _notificador.AddMensagemNaoEncontrado(NotificacoesUsuario.ChaveAcessoNaoEncontrada);
        }

        public bool AlteracaoNomeEhValida(UsuarioAlterarNome usuarioAlterarNome)
        {
            if (!UsuarioValidador.NomeEhValido(usuarioAlterarNome.Nome))
            {
                _notificador.AddMensagem(NotificacoesUsuario.NomeInvalido);
                return false;
            }

            return true;
        }

        public void UsuarioNaoEncontrado()
        {
            _notificador.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado);
        }

        public bool AlteracaoEmailEhValida(UsuarioAlterarEmail usuarioAlterarEmail)
        {
            if (!UsuarioValidador.EmailEhValido(usuarioAlterarEmail.Email))
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailInvalido);
                return false;
            }

            return true;
        }

        public bool AutenticacaoEhValida(UsuarioAutenticar usuarioAutenticar, Usuario usuario)
        {
            var senhasIguais = Criptografia.SaoIguais(usuarioAutenticar.Senha, usuario.Senha);
            if (!senhasIguais)
            {
                _notificador.AddMensagemNaoAutorizado(NotificacoesUsuario.SenhaIncorreta);
                return false;
            }

            return true;
        }
    }
}
