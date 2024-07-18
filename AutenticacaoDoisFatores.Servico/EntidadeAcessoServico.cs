using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
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
            if (!CadastroEhValido(entidadeAcessoCadastrar))
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
        private bool CadastroEhValido(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            if (entidadeAcessoCadastrar.Nome.IsNullOrEmptyOrWhiteSpaces())
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido);
                return false;
            }

            if (entidadeAcessoCadastrar.Email.IsNullOrEmptyOrWhiteSpaces())
            {
                _notificador.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido);
                return false;
            }

            return true;
        }

        private static void VerificarChaveAcesso(string chave)
        {
            if (chave.IsNullOrEmptyOrWhiteSpaces())
                throw new ApplicationException("Não foi possível recuperar a chave de acesso");
        }
    }
}
