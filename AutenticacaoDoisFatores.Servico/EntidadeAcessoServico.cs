using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
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

        public async Task AlterarChaveAcessoAsync(string token)
        {
            var email = Token.RetornarEmailReenvio(token) ?? "";
            var entidadeAcesso = await _dominio.GerarNovaChaveAsync(email);
            if (!GeracaoChaveValida(entidadeAcesso))
                return;

            var chaveSemCriptografia = entidadeAcesso?.RetornarChaveSemCriptografia() ?? "";
            EmailServico.ReenviarChaveDeAcesso(email, chaveSemCriptografia);
        }

        public async Task AtivarCadastroAsync(string token)
        {
            var email = Token.RetornarEmailEnvio(token) ?? "";
            if (!await AtivacaoEhValidaAsync(email))
                return;

            await _dominio.AtivarEntidadeAcessoAsync(email, true);
        }

        public async Task<EntidadeAcessoCadastrada?> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar, string urlBase)
        {
            if (!await CadastroEhValidoAsync(entidadeAcessoCadastrar))
                return null;

            var entidadeAcessoMapeada = _mapeador.Map<EntidadeAcesso>(entidadeAcessoCadastrar);

            var entidadeAcesso = await _dominio.CadastrarAsync(entidadeAcessoMapeada);

            var chave = entidadeAcesso.RetornarChaveSemCriptografia();
            VerificarChaveAcesso(chave);

            var token = Token.GerarTokenEnvioChave(entidadeAcesso.Email);
            var urlConfirmacaoCadastro = $"{urlBase}{token}";

            EmailServico.EnviarSucessoCadastroDeAcesso(entidadeAcesso.Email, chave, urlConfirmacaoCadastro);

            var entidadeAcesssoCadastrada = _mapeador.Map<EntidadeAcessoCadastrada>(entidadeAcesso);

            return entidadeAcesssoCadastrada;
        }

        public async Task ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso, string urlBase)
        {
            if (!ReenvioEhValido(reenviarChaveAcesso))
                return;

            if (!await EntidadeExisteAsync(reenviarChaveAcesso.Email))
                return;

            var token = Token.GerarTokenReenvioChave(reenviarChaveAcesso.Email);
            var urlConfirmacaoGeracaoNovaChave = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoChaveAcesso(reenviarChaveAcesso.Email, urlConfirmacaoGeracaoNovaChave);
        }
    }

    public partial class EntidadeAcessoServico
    {
        private readonly INotificador _notificador = notificador;

        private async Task<bool> CadastroEhValidoAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            var nome = entidadeAcessoCadastrar.Nome;
            if (nome.IsNullOrEmptyOrWhiteSpaces() || nome.Length < 3 || nome.Length > 50)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido);
                return false;
            }

            var email = entidadeAcessoCadastrar.Email;
            if (email.IsNullOrEmptyOrWhiteSpaces() || email.Length < 5 || email.Length > 80)
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

        private static void VerificarChaveAcesso(string chave)
        {
            if (chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoServicoException.FalhaAoRecuperarChaveAcesso();
        }

        private bool ReenvioEhValido(ReenviarChaveAcesso? reenviarChaveAcesso)
        {
            if (reenviarChaveAcesso?.Email.IsNullOrEmptyOrWhiteSpaces() ?? false)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
                return false;
            }

            return true;
        }

        private async Task<bool> EntidadeExisteAsync(string email)
        {
            var existe = await _dominio.ExisteEntidadeComEmailAsync(email);
            if (!existe)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NaoEncontrada);
                return false;
            }

            return true;
        }

        private async Task<bool> AtivacaoEhValidaAsync(string email)
        {
            var entidadeAcesso = await _dominio.BuscarComEmailAsync(email);
            if (entidadeAcesso is null)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NaoEncontrada);
                return false;
            }

            if (entidadeAcesso.Ativo)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.JaAtiva);
                return false;
            }

            return true;
        }

        private bool GeracaoChaveValida(EntidadeAcesso? entidadeAcesso)
        {
            if (entidadeAcesso is null)
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NaoEncontrada);
                return false;
            }

            if (entidadeAcesso.Chave.IsNullOrEmptyOrWhiteSpaces())
                EntidadeAcessoServicoException.FalhaAoRecuperarChaveAcesso();

            return true;
        }
    }
}
