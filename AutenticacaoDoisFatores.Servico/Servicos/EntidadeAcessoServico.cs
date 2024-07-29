using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Servico.Validacoes;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class EntidadeAcessoServico(IEntidadeAcessoDominio _dominio, IMapper _mapeador, IEmailServico _email, EntidadeAcessoServicoValidacao _validador) : IEntidadeAcessoServico
    {
        public async Task<EntidadeAcessoResposta?> CadastrarAsync(EntidadeAcessoCadastrar entidadeCadastrar, string urlBase)
        {
            if (!await _validador.CadastroEhValidoAsync(entidadeCadastrar))
                return null;

            var chave = ChaveAcesso.GerarChave();
            var chaveCriptografada = Criptografia.Criptografar(chave);

            var entidade = new EntidadeAcesso(entidadeCadastrar.Nome, chaveCriptografada, entidadeCadastrar.Email);
            var entidadeCadastrada = await _dominio.CadastrarAsync(entidade);

            var token = Token.GerarTokenEnvioChaveAcesso(entidade.Email);
            var urlConfirmacaoCadastro = $"{urlBase}{token}";

            _email.EnviarSucessoCadastroDeAcesso(entidade.Email, chave, urlConfirmacaoCadastro);

            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidadeCadastrada);

            return entidadeResposta;
        }

        public async Task<EntidadeAcessoResposta?> AtivarCadastroAsync(string token)
        {
            var email = Token.RetornarEmailConfirmacaoCadastroEntidadeAcesso(token) ?? "";

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
            var entidade = await _dominio.BuscarComEmailAsync(reenviarChave.Email);
            if (entidade is null)
                return false;

            var token = Token.GerarTokenReenvioChave(entidade.Id);
            var urlConfirmacaoGeracaoNovaChave = $"{urlBase}{token}";

            _email.EnviarConfirmacaoAlteracaoChaveAcesso(reenviarChave.Email, urlConfirmacaoGeracaoNovaChave);

            return true;
        }

        public async Task<bool> AlterarChaveAcessoAsync(string token)
        {
            var id = Token.RetornarIdReenvioChave(token) ?? 0;

            var entidadeParaAlterar = await _dominio.BuscarAsync(id);
            if (entidadeParaAlterar is null)
                return false;

            var chave = ChaveAcesso.GerarChave();
            var chaveCriptografada = Criptografia.Criptografar(chave);
            entidadeParaAlterar.AlterarChave(chaveCriptografada);
            await _dominio.AlterarAsync(entidadeParaAlterar);

            _email.ReenviarChaveDeAcesso(entidadeParaAlterar.Email, chave);

            return true;
        }

        public async Task<bool> EnviarEmailAlteracaoNomeAsync(EntidadeAcessoAlterarNome entidadeAlterar, string urlBase)
        {
            var entidade = await _dominio.BuscarComEmailAsync(entidadeAlterar.Email);
            if (entidade is null)
                return false;

            if (!_validador.AlteracaoNomeEhValida(entidadeAlterar))
                return false;

            var token = Token.GerarTokenAlterarNomeEntidadeAcesso(entidade.Id, entidadeAlterar.Nome);
            var linkConfirmacao = $"{urlBase}{token}";

            _email.EnviarConfirmacaoAlteracaoEntidadeAcesso(entidade.Email, linkConfirmacao);

            return true;
        }

        public async Task<EntidadeAcessoResposta?> AlterarNomeAsync(string token)
        {
            var dadosToken = Token.RetornarNomeAlteracaoEntidadeAcesso(token);
            var id = dadosToken.id ?? 0;
            var novoNome = dadosToken.nome ?? "";

            var entidadeParaAlterar = await _dominio.BuscarAsync(id);
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

            if (!_validador.AlteracaoEmailEhValida(entidadeAlterarEmail))
                return false;

            var chaveRecebida = entidadeAlterarEmail.Chave;
            if (!Criptografia.SaoIguais(chaveRecebida, entidade.Chave))
                return false;

            var token = Token.GerarTokenAlterarEmailEntidadeAcesso(entidade.Id, entidadeAlterarEmail.EmailNovo);
            var linkConfirmacao = $"{urlBase}{token}";

            _email.EnviarConfirmacaoAlteracaoEntidadeAcesso(entidadeAlterarEmail.EmailNovo, linkConfirmacao);

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
