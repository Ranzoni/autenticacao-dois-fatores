using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using AutenticacaoDoisFatores.Servico.Validacoes;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico
{
    public class EntidadeAcessoServico(IEntidadeAcessoDominio dominio, IMapper mapeador, EntidadeAcessoServicoValidacao validador) : IEntidadeAcessoServico
    {
        protected readonly IEntidadeAcessoDominio _dominio = dominio;
        private readonly IMapper _mapeador = mapeador;
        private readonly EntidadeAcessoServicoValidacao _validador = validador;

        public async Task<EntidadeAcessoResposta?> CadastrarAsync(EntidadeAcessoCadastrar entidadeCadastrar, string urlBase)
        {
            if (!await _validador.CadastroEhValidoAsync(entidadeCadastrar))
                return null;

            var chave = ChaveAcesso.GerarChave();
            var chaveCriptografada = Criptografia.Criptografar(chave);

            var entidade = new EntidadeAcesso(entidadeCadastrar.Nome, chaveCriptografada, entidadeCadastrar.Email);
            var entidadeCadastrada = await _dominio.CadastrarAsync(entidade);

            var token = Token.GerarTokenEnvioChave(entidade.Email);
            var urlConfirmacaoCadastro = $"{urlBase}{token}";

            EmailServico.EnviarSucessoCadastroDeAcesso(entidade.Email, chave, urlConfirmacaoCadastro);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidadeCadastrada);

            return entidadeResposta;
        }

        public async Task<EntidadeAcessoResposta?> AtivarCadastroAsync(string token)
        {
            var email = Token.RetornarEmailEnvioConfirmacaoCadastro(token) ?? "";

            var entidadeParaAlterar = await _dominio.BuscarComEmailAsync(email);
            if (entidadeParaAlterar is null)
                return null;

            if (!_validador.AtivacaoEhValida(entidadeParaAlterar))
                return null;

            entidadeParaAlterar.Ativar(true);
            var entidadeAlterada = await _dominio.AlterarAsync(entidadeParaAlterar);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidadeAlterada);

            return entidadeResposta;
        }

        public async Task<bool> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChave, string urlBase)
        {
            var existeEntidade = await _dominio.ExisteEntidadeComEmailAsync(reenviarChave.Email);
            if (!existeEntidade)
                return false;

            var token = Token.GerarTokenReenvioChave(reenviarChave.Email);
            var urlConfirmacaoGeracaoNovaChave = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoChaveAcesso(reenviarChave.Email, urlConfirmacaoGeracaoNovaChave);

            return true;
        }

        public async Task<bool> AlterarChaveAcessoAsync(string token)
        {
            var email = Token.RetornarEmailReenvioChave(token) ?? "";

            var entidadeParaAlterar = await _dominio.BuscarComEmailAsync(email);
            if (entidadeParaAlterar is null)
                return false;

            var chave = ChaveAcesso.GerarChave();
            var chaveCriptografada = Criptografia.Criptografar(chave);
            entidadeParaAlterar.AlterarChave(chaveCriptografada);
            await _dominio.AlterarAsync(entidadeParaAlterar);

            EmailServico.ReenviarChaveDeAcesso(email, chave);

            return true;
        }

        public async Task<bool> EnviarEmailAlteracaoNomeAsync(EntidadeAcessoAlterarNome entidadeAlterar, string urlBase)
        {
            var existeEntidade = await _dominio.ExisteEntidadeComEmailAsync(entidadeAlterar.Email);
            if (!existeEntidade)
                return false;

            var token = Token.GerarTokenAlterarNomeEntidadeAcesso(entidadeAlterar.Email, entidadeAlterar.Nome);
            var linkConfirmacao = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoEntidadeAcesso(entidadeAlterar.Email, linkConfirmacao);

            return true;
        }

        public async Task<EntidadeAcessoResposta?> AlterarNomeAsync(string token)
        {
            var dadosToken = Token.RetornarEmailNomeAlteracaoEntidadeAcesso(token);
            var email = dadosToken.email ?? "";
            var novoNome = dadosToken.nome ?? "";

            var entidadeParaAlterar = await _dominio.BuscarComEmailAsync(email);
            if (entidadeParaAlterar is null)
                return null;

            entidadeParaAlterar.AlterarNome(novoNome);
            var entidadeAlterada = await _dominio.AlterarAsync(entidadeParaAlterar);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta?>(entidadeAlterada);

            return entidadeResposta;
        }

        public async Task<bool> EnviarEmailAlteracaoEmailAsync(EntidadeAcessoAlterarEmail entidadeAlterarEmail, string urlBase)
        {
            var entidade = await _dominio.BuscarComEmailAsync(entidadeAlterarEmail.EmailAtual);
            if (entidade is null)
                return false;

            var chaveRecebida = entidadeAlterarEmail.Chave;
            if (!Criptografia.SaoIguais(chaveRecebida, entidade.Chave))
                return false;

            var token = Token.GerarTokenAlterarEmailEntidadeAcesso(entidade.Id, entidadeAlterarEmail.EmailNovo);
            var linkConfirmacao = $"{urlBase}{token}";

            EmailServico.EnviarConfirmacaoAlteracaoEntidadeAcesso(entidadeAlterarEmail.EmailNovo, linkConfirmacao);

            return true;
        }

        public async Task<EntidadeAcessoResposta?> AlterarEmailAsync(string token)
        {
            var dadosToken = Token.RetornarIdEmailAlteracaoEmailEntidadeAcesso(token);
            var id = dadosToken.id ?? 0;
            var novoEmail = dadosToken.email ?? "";

            var entidadeParaAlterar = await _dominio.BuscarAsync(id);
            if (entidadeParaAlterar is null)
                return null;

            entidadeParaAlterar.AlterarEmail(novoEmail);
            var entidadeAlterada = await _dominio.AlterarAsync(entidadeParaAlterar);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta?>(entidadeAlterada);

            return entidadeResposta;
        }
    }
}
