using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Autenticacao;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Servico.Validacoes;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class AutenticacaoServico(IUsuarioDominio _dominio, AutenticacaoServicoValidacao _validacao) : IAutenticacaoServico
    {
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

        public async Task<bool> InativarAsync(int id, Guid chave)
        {
            var usuario = await _dominio.BuscarAtivoAsync(id, chave);
            if (usuario is null)
            {
                _validacao.UsuarioNaoEncontrado();
                return false;
            }

            usuario.Ativar(false);
            await _dominio.AlterarAsync(usuario);

            return true;
        }
    }
}
