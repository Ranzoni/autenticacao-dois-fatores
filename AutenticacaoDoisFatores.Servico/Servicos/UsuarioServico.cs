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
        public async Task<bool> AtivarAsync(string token)
        {
            var id = Token.RetornarIdConfirmacaoCadastro(token) ?? 0;

            var usuario = await _dominio.BuscarAsync(id);
            if (usuario is null)
                return false;

            if (!_validacao.AtivacaoEhValida(usuario))
                return false;

            usuario.Ativar(true);
            await _dominio.AlterarAsync(usuario);

            return true;
        }

        public async Task<UsuarioResposta?> CadastrarAsync(UsuarioCadastrar usuarioCadastrar, string urlBase)
        {
            var entidadeAcesso = await _dominioAcesso.BuscarComChaveAsync(usuarioCadastrar.Chave);
            if (entidadeAcesso is null)
                return null;

            if (!await _validacao.CadastroEhValidoAsync(usuarioCadastrar))
                return null;

            var senhaCriptografada = Criptografia.Criptografar(usuarioCadastrar.Senha);
            var novoUsuario = new Usuario(usuarioCadastrar.Nome, usuarioCadastrar.Email, senhaCriptografada, entidadeAcesso);

            await _dominio.CadastrarAsync(novoUsuario);

            var token = Token.GerarTokenConfirmacaoCadastro(novoUsuario.Id);
            var linkConfirmacao = $"{urlBase}{token}";
            _email.EnviarEmailConfirmacaoCadastro(novoUsuario.Email, linkConfirmacao);

            var usuarioResposta = _mapeador.Map<UsuarioResposta>(novoUsuario);

            return usuarioResposta;
        }
    }
}
