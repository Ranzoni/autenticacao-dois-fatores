using AutenticacaoDoisFatores.Controllers.Base;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using Mensageiro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController(INotificador notificador, IConfiguration configuration, IUsuarioServico _servico, IAutenticacaoServico _servicoAutenticacao) : BaseController(notificador, configuration)
    {
        [HttpPost("Cadastrar")]
        public async Task<ActionResult<UsuarioResposta?>> CadastrarAsync(UsuarioCadastrar usuarioCadastrar)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("Usuario/ConfirmarCadastro/");

                var retorno = await _servico.CadastrarAsync(usuarioCadastrar, urlBase);

                return CriadoComSucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarCadastro/{token}")]
        public async Task<ContentResult> ConfirmarCadastroAsync(string token)
        {
            try
            {
                var retorno = await _servicoAutenticacao.AtivarAsync(token);
                if (!retorno)
                    return MensagemHtml("Falha", "Não encontrada", "Não foi encontrado um usuário com este token");

                return MensagemHtml("Confirmação", "Confirmação de cadastro", "O cadastro foi ativado com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", $"Por favor, entre em contato com o responsável pelo sistema. Erro: {e.Message}");
            }
        }

        [Authorize]
        [HttpPut("Alterar")]
        public async Task<ActionResult<UsuarioResposta?>> AlterarAsync(UsuarioAlterar usuarioAlterar)
        {
            try
            {
                var (id, chave) = RetornarDadosTokenAutenticacao();

                var retorno = await _servico.AlterarAsync(id, chave, usuarioAlterar);

                return Sucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("AlterarEmail")]
        public async Task<ActionResult> AlterarEmailAsync(UsuarioAlterarEmail usuarioAlterar)
        {
            try
            {
                var (id, chave) = RetornarDadosTokenAutenticacao();

                var urlBase = RetornarUrlFormatada("Usuario/ConfirmarAlteracaoEmail/");
                var retorno = await _servico.EnviarEmailAlteracaoEmailAsync(id, chave, usuarioAlterar, urlBase);

                return Sucesso("Um e-mail de confirmação foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarAlteracaoEmail/{token}")]
        public async Task<ActionResult> ConfirmarAlteracaoEmailAsync(string token)
        {
            try
            {
                var retorno = await _servico.AlterarEmailAsync(token);
                if (retorno is null)
                    return MensagemHtml("Falha", "Não encontrado", "Não foi encontrado um usuário com estes dados.");

                return MensagemHtml("Confirmação", "Confirmação de alteração", "A alteração foi realizada com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", $"Por favor, entre em contato com o responsável pelo sistema. Erro: {e.Message}");
            }
        }

        [HttpPut("AlterarSenha/{id}")]
        public async Task<ActionResult> AlterarEmailAsync(int id, UsuarioAlterarSenha usuarioAlterar)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("Usuario/ConfirmarAlteracaoSenha/");
                var retorno = await _servico.EnviarEmailAlteracaoSenhaAsync(id, usuarioAlterar, urlBase);

                return Sucesso("Um e-mail de confirmação foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarAlteracaoSenha/{token}")]
        public async Task<ActionResult> ConfirmarAlteracaoSenhaAsync(string token)
        {
            try
            {
                var retorno = await _servico.AlterarSenhaAsync(token);
                if (!retorno)
                    return MensagemHtml("Falha", "Não encontrado", "Não foi encontrado um usuário com estes dados.");

                return MensagemHtml("Confirmação", "Confirmação de alteração", "A alteração foi realizada com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", $"Por favor, entre em contato com o responsável pelo sistema. Erro: {e.Message}");
            }
        }
    }
}
