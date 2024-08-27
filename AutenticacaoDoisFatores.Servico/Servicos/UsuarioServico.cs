using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Servico.Validacoes;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class UsuarioServico(IUsuarioDominio _dominio, IEntidadeAcessoDominio _dominioAcesso, IMapper _mapeador, UsuarioServicoValidacao _validacao, IEmailServico _email) : IUsuarioServico
    {
        public async Task<bool> EnviarEmailAlteracaoEmailAsync(int id, UsuarioAlterarEmail usuarioAlterarEmail, string urlBase)
        {
            if (!_validacao.AlteracaoEmailEhValida(usuarioAlterarEmail))
                return false;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, usuarioAlterarEmail.Chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return false;
            }

            var token = Token.GerarTokenAlterarEmailUsuario(id, usuarioAlterarEmail.Email, usuarioAlterarEmail.Chave);
            var linkConfirmacao = $"{urlBase}{token}";
            _email.EnviarEmailConfirmacaoAlteracaoEmail(usuarioAlterarEmail.Email, usuarioCadastrado.Nome, linkConfirmacao);

            return true;
        }

        public async Task<UsuarioResposta?> AlterarNomeAsync(int id, UsuarioAlterarNome usuarioAlterarNome)
        {
            if (!_validacao.AlteracaoNomeEhValida(usuarioAlterarNome))
                return null;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, usuarioAlterarNome.Chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return null;
            }

            usuarioCadastrado.AlterarNome(usuarioAlterarNome.Nome);

            await _dominio.AlterarAsync(usuarioCadastrado);

            var usuarioResposta = _mapeador.Map<UsuarioResposta>(usuarioCadastrado);

            return usuarioResposta;
        }

        public async Task<bool> AtivarAsync(string token)
        {
            var dadosToken = Token.RetornarIdEChaveConfirmacaoCadastro(token);
            var id = dadosToken.id ?? 0;
            var chave = dadosToken.chave ?? Guid.Empty;

            var usuario = await _dominio.BuscarNaoAtivoAsync(id, chave);
            if (!_validacao.AtivacaoEhValida(usuario))
                return false;

            if (usuario is null)
                return false;

            usuario.Ativar(true);
            await _dominio.AlterarAsync(usuario);

            return true;
        }

        public async Task<UsuarioResposta?> CadastrarAsync(UsuarioCadastrar usuarioCadastrar, string urlBase)
        {
            var entidadeAcesso = await _dominioAcesso.BuscarComChaveAsync(usuarioCadastrar.Chave);
            if (entidadeAcesso is null)
            {
                _validacao.ChaveAcessoNaoEncontrado();
                return null;
            }

            if (!await _validacao.CadastroEhValidoAsync(usuarioCadastrar))
                return null;

            var senhaCriptografada = Criptografia.Criptografar(usuarioCadastrar.Senha);
            var novoUsuario = new Usuario(usuarioCadastrar.Nome, usuarioCadastrar.Email, senhaCriptografada, entidadeAcesso);

            await _dominio.CadastrarAsync(novoUsuario);

            var token = Token.GerarTokenConfirmacaoCadastro(novoUsuario.Id, entidadeAcesso.Chave);
            var linkConfirmacao = $"{urlBase}{token}";
            _email.EnviarEmailConfirmacaoCadastro(novoUsuario.Email, linkConfirmacao);

            var usuarioResposta = _mapeador.Map<UsuarioResposta>(novoUsuario);

            return usuarioResposta;
        }

        public async Task<UsuarioResposta?> AlterarEmailAsync(string token)
        {
            var dadosToken = Token.RetornarIdEmailAlteracaoEmailUsuario(token);
            var id = dadosToken.id ?? 0;
            var email = dadosToken.email ?? "";
            var chave = dadosToken.chave ?? Guid.Empty;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return null;
            }

            usuarioCadastrado.AlterarEmail(email);

            await _dominio.AlterarAsync(usuarioCadastrado);

            var usuarioResposta = _mapeador.Map<UsuarioResposta>(usuarioCadastrado);

            return usuarioResposta;
        }

        public async Task<UsuarioAutenticado?> AutenticarAsync(UsuarioAutenticar usuarioAutenticar)
        {
            var usuario = await _dominio.BuscarPorEmailAsync(usuarioAutenticar.Email, usuarioAutenticar.Chave);
            if (usuario is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return null;
            }

            if (!_validacao.AutenticacaoEhValida(usuarioAutenticar, usuario))
                return null;

            var token = Token.GerarTokenAutenticacaoUsuario(usuario.Id, usuarioAutenticar.Chave);

            var usuarioAutenticado = new UsuarioAutenticado(usuario.Id, usuario.Email, token);

            return usuarioAutenticado;
        }
    }
}
