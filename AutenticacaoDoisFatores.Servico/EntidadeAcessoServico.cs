using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Core.Validadores;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Excecoes;
using AutenticacaoDoisFatores.Servico.Interfaces;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico
{
    public partial class EntidadeAcessoServico(IEntidadeAcessoDominio dominio, IMapper mapeador, INotificador notificador) : IEntidadeAcessoServico
    {
        protected readonly IEntidadeAcessoDominio _dominio = dominio;
        private readonly IMapper _mapeador = mapeador;

        public async Task<EntidadeAcessoResposta?> CadastrarAsync(EntidadeAcessoCadastrar entidadeCadastrar, string urlBase)
        {
            if (!await CadastroEhValidoAsync(entidadeCadastrar))
                return null;

            var entidade = _mapeador.Map<EntidadeAcesso>(entidadeCadastrar);

            var entidadeCadastrada = await _dominio.CadastrarAsync(entidade);

            var token = Token.GerarTokenEnvioChave(entidade.Email);
            var urlConfirmacaoCadastro = $"{urlBase}{token}";

            EmailServico.EnviarSucessoCadastroDeAcesso(entidade.Email, entidade.Chave, urlConfirmacaoCadastro);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidadeCadastrada);

            return entidadeResposta;
        }

        public async Task<EntidadeAcessoResposta?> AtivarCadastroAsync(string token)
        {
            var email = Token.RetornarEmailEnvioConfirmacaoCadastro(token) ?? "";
            if (email.IsNullOrEmptyOrWhiteSpaces())
            {
                NotificarEmailInvalido();
                return null;
            }

            if (!await AtivacaoEhValidaAsync(email))
                return null;

            var entidade = await _dominio.AtivarEntidadeAcessoAsync(email, true);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidade);

            return entidadeResposta;
        }

        public async Task<bool> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChave, string urlBase)
        {
            if (!await EntidadeExisteAsync(reenviarChave.Email))
                return false;

            var token = Token.GerarTokenReenvioChave(reenviarChave.Email);
            var urlConfirmacaoGeracaoNovaChave = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoChaveAcesso(reenviarChave.Email, urlConfirmacaoGeracaoNovaChave);

            return true;
        }

        public async Task<bool> AlterarChaveAcessoAsync(string token)
        {
            var email = Token.RetornarEmailReenvioChave(token) ?? "";

            var chave = await _dominio.GerarNovaChaveAsync(email) ?? "";
            if (chave.IsNullOrEmptyOrWhiteSpaces())
                return false;

            EmailServico.ReenviarChaveDeAcesso(email, chave);

            return true;
        }

        public async Task<bool> EnviarEmailAlteracaoNomeAsync(EntidadeAcessoAlterar entidadeAlterar, string urlBase)
        {
            if (!await EnvioAlteracaoEhValidoAsync(entidadeAlterar))
                return false;

            var token = Token.GerarTokenAlterarEntidadeAcesso(entidadeAlterar.Email, entidadeAlterar.Nome);
            var linkConfirmacao = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoEntidadeAcesso(entidadeAlterar.Email, linkConfirmacao);

            return true;
        }

        public async Task<EntidadeAcessoResposta?> AlterarNomeAsync(string token)
        {
            var dadosToken = Token.RetornarEmailNomeAlteracaoCadastro(token);
            var email = dadosToken.email ?? "";
            var nome = dadosToken.nome ?? "";

            var entidade = await _dominio.AlterarNomeAsync(email, nome);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta?>(entidade);

            return entidadeResposta;
        }

        public async Task<EntidadeAcessoResposta?> AlterarEmailAsync(string emailAtual, EntidadeAcessoAlterarEmail entidadeAcessoAlterarEmail)
        {
            if (!AlteracaoEmailEhValida(emailAtual, entidadeAcessoAlterarEmail.Chave, entidadeAcessoAlterarEmail.Email))
                return null;

            var entidadeAcesso = await _dominio.AlterarEmailAsync(emailAtual, entidadeAcessoAlterarEmail.Chave, entidadeAcessoAlterarEmail.Email);
            if (entidadeAcesso is null)
                NotificarNaoEncontrada();

            var entidadeMapeada = _mapeador.Map<EntidadeAcessoResposta>(entidadeAcesso);

            return entidadeMapeada;
        }
    }

    public partial class EntidadeAcessoServico
    {
        private readonly INotificador _notificador = notificador;

        private async Task<bool> CadastroEhValidoAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
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

        private async Task<bool> EntidadeExisteAsync(string email)
        {
            var existe = await _dominio.ExisteEntidadeComEmailAsync(email);
            if (!existe)
                return false;

            return true;
        }

        private async Task<bool> AtivacaoEhValidaAsync(string email)
        {
            var entidadeAcesso = await _dominio.BuscarComEmailAsync(email);
            if (entidadeAcesso is null)
                return false;

            if (entidadeAcesso.Ativo)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.JaAtiva);
                return false;
            }

            return true;
        }

        private async Task<bool> EnvioAlteracaoEhValidoAsync(EntidadeAcessoAlterar entidadeAcessoAlterar)
        {
            if (!EntidadeAcessoValidador.EmailEhValido(entidadeAcessoAlterar.Email))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
                return false;
            }

            if (!EntidadeAcessoValidador.NomeEhValido(entidadeAcessoAlterar.Nome))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido);
                return false;
            }

            if (!await EntidadeExisteAsync(entidadeAcessoAlterar.Email))
                return false;

            return true;
        }

        private bool AlteracaoEmailEhValida(string emailAtual, string chave, string emailNovo)
        {
            if (!EntidadeAcessoValidador.EmailEhValido(emailAtual))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailAtualInvalido);
                return false;
            }

            if (!EntidadeAcessoValidador.ChaveEhValida(chave))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.ChaveInvalida);
                return false;
            }

            if (!EntidadeAcessoValidador.EmailEhValido(emailNovo))
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailNovoInvalido);
                return false;
            }

            return true;
        }
        
        private void NotificarNaoEncontrada()
        {
            _notificador.AddMensagem(NotificacoesEntidadeAcesso.NaoEncontrada);
        }

        private void NotificarEmailInvalido()
        {
            _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
        }
    }
}
