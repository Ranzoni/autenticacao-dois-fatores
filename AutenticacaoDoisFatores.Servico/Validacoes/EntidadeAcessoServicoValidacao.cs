using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Core.Validadores;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public class EntidadeAcessoServicoValidacao(INotificadorServico notificador, IEntidadeAcessoDominio dominio) : BaseValidacao(notificador)
    {
        private readonly IEntidadeAcessoDominio _dominio = dominio;

        public async Task<bool> CadastroEhValidoAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            if (!EntidadeAcessoValidador.NomeEhValido(entidadeAcessoCadastrar.Nome))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido);
                return false;
            }

            if (!EntidadeAcessoValidador.EmailEhValido(entidadeAcessoCadastrar.Email))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
                return false;
            }

            var emailJaCadastrado = await _dominio.ExisteEntidadeComEmailAsync(entidadeAcessoCadastrar.Email);
            if (emailJaCadastrado)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailJaCadastrado);
                return false;
            }

            return true;
        }

        public bool AtivacaoEhValida(EntidadeAcesso entidadeAcesso)
        {
            if (entidadeAcesso.Ativo)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.JaAtiva);
                return false;
            }

            return true;
        }

        public bool AlteracaoNomeEhValida(EntidadeAcessoAlterarNome entidadeAlterar)
        {
            if (!EntidadeAcessoValidador.NomeEhValido(entidadeAlterar.Nome))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido);
                return false;
            }

            return true;
        }

        public bool AlteracaoEmailEhValida(EntidadeAcessoAlterarEmail entidadeAlterarEmail)
        {
            if (!EntidadeAcessoValidador.EmailEhValido(entidadeAlterarEmail.EmailNovo))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
                return false;
            }

            return true;
        }
    }
}
