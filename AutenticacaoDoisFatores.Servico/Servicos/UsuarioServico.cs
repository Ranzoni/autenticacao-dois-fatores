using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
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
        public async Task<bool> EnviarEmailAlteracaoEmailAsync(int id, Guid chave, UsuarioAlterarEmail usuarioAlterarEmail, string urlBase)
        {
            if (!_validacao.AlteracaoEmailEhValida(usuarioAlterarEmail))
                return false;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return false;
            }

            var token = Token.GerarTokenAlterarEmailUsuario(id, usuarioAlterarEmail.Email, chave);
            var linkConfirmacao = $"{urlBase}{token}";
            _email.EnviarEmailConfirmacaoAlteracaoEmail(usuarioAlterarEmail.Email, usuarioCadastrado.Nome, linkConfirmacao);

            return true;
        }

        public async Task<UsuarioResposta?> AlterarAsync(int id, Guid chave, UsuarioAlterar usuarioAlterar)
        {
            if (!_validacao.AlteracaoEhValida(usuarioAlterar))
                return null;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return null;
            }

            if (usuarioAlterar.Nome is not null)
                usuarioCadastrado.AlterarNome(usuarioAlterar.Nome);

            if (usuarioAlterar.Senha is not null)
            {
                var senhaCriptografada = Criptografia.Criptografar(usuarioAlterar.Senha);
                usuarioCadastrado.AlterarSenha(senhaCriptografada);
            }

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

            var senhasIguais = Criptografia.SaoIguais(usuarioAutenticar.Senha, usuario.Senha);
            if (!senhasIguais)
            {
                _validacao.NaoAutorizado();
                return null;
            }

            var token = Token.GerarTokenAutenticacaoUsuario(usuario.Id, usuarioAutenticar.Chave);

            usuario.AtualizarDataUltimoAcesso();
            await _dominio.AlterarAsync(usuario);

            var usuarioAutenticado = new UsuarioAutenticado(usuario.Id, usuario.Email, token);

            return usuarioAutenticado;
        }

        public async Task<bool> EnviarEmailAlteracaoSenhaAsync(int id, UsuarioAlterarSenha usuarioAlterarSenha, string urlBase)
        {
            if (!_validacao.AlteracaoSenhaEhValida(usuarioAlterarSenha))
                return false;

            var usuario = await _dominio.BuscarAsync(id, usuarioAlterarSenha.Chave);
            if (usuario is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return false;
            }

            var token = Token.GerarTokenAlterarSenhaUsuario(id, usuarioAlterarSenha.Senha, usuarioAlterarSenha.Chave);
            var linkConfirmacao = $"{urlBase}{token}";

            _email.EnviarEmailConfirmacaoAlteracaoSenha(usuario.Email, usuario.Nome, linkConfirmacao);

            return true;
        }

        public async Task<bool> AlterarSenhaAsync(string token)
        {
            var dadosToken = Token.RetornarIdSenhaAlteracaoSenhaUsuario(token);
            var id = dadosToken.id ?? 0;
            var senha = dadosToken.senha ?? "";
            var chave = dadosToken.chave ?? Guid.Empty;

            var usuarioCadastrado = await _dominio.BuscarAsync(id, chave);
            if (usuarioCadastrado is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return false;
            }

            var senhaCriptografada = Criptografia.Criptografar(senha);
            usuarioCadastrado.AlterarSenha(senhaCriptografada);

            await _dominio.AlterarAsync(usuarioCadastrado);

            return true;
        }
    }
}
