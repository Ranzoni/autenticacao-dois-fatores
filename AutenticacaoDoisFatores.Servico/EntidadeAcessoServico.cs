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
        private readonly INotificador _notificador = notificador;

        public async Task<EntidadeAcessoCadastrada?> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            if (!await CadastroEhValidoAsync(entidadeAcessoCadastrar))
                return null;

            var entidadeAcessoMapeada = _mapeador.Map<EntidadeAcesso>(entidadeAcessoCadastrar);

            var entidadeAcesso = await _dominio.CadastrarAsync(entidadeAcessoMapeada);

            var chave = entidadeAcesso.RetornarChaveSemCriptografia();
            VerificarChaveAcesso(chave);

            EmailServico.EnviarSucessoCadastroDeAcesso(entidadeAcesso.Email, chave);

            var entidadeAcesssoCadastrada = _mapeador.Map<EntidadeAcessoCadastrada>(entidadeAcesso);

            return entidadeAcesssoCadastrada;
        }
    }

    public partial class EntidadeAcessoServico
    {
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
    }
}
