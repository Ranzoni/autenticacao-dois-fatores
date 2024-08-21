using AutenticacaoDoisFatores.Controllers.Base;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using Mensageiro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadeAcessoController(IEntidadeAcessoServico _servico, INotificador notificador, IConfiguration config) : BaseController(notificador, config)
    {
        [HttpPost("Cadastrar")]
        public async Task<ActionResult<EntidadeAcessoResposta?>> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("EntidadeAcesso/ConfirmarCadastro/");

                var retorno = await _servico.CadastrarAsync(entidadeAcessoCadastrar, urlBase);

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
                var retorno = await _servico.AtivarCadastroAsync(token);
                if (retorno is null)
                    return MensagemHtml("Falha", "N�o encontrada", "N�o foi encontrada uma entidade com este endere�o de e-mail");

                return MensagemHtml("Confirma��o", "Confirma��o de cadastro", "O cadastro foi ativado com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Ser� necess�rio solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicita��o", $"Por favor, entre em contato com o respons�vel pelo sistema. Erro: {e.Message}");
            }
        }

        [HttpPut("ReenviarChaveAcesso")]
        public async Task<ActionResult> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("EntidadeAcesso/GerarNovaChaveAcesso/");

                var retorno = await _servico.ReenviarChaveAcessoAsync(reenviarChaveAcesso, urlBase);

                return Sucesso("Um e-mail de recupera��o foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GerarNovaChaveAcesso/{token}")]
        public async Task<ContentResult> GerarNovaChaveAcessoAsync(string token)
        {
            try
            {
                var retorno = await _servico.AlterarChaveAcessoAsync(token);
                if (!retorno)
                    return MensagemHtml("Falha", "N�o encontrada", "N�o foi encontrada uma entidade com este endere�o de e-mail");

                return MensagemHtml("Confirma��o", "Confirma��o de reenvio de chave", "Foi enviado um novo e-mail com a nova chave de acesso");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Ser� necess�rio solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicita��o", $"Por favor, entre em contato com o respons�vel pelo sistema. Erro: {e.Message}");
            }
        }

        [HttpPut("AlterarNome")]
        public async Task<ActionResult> AlterarNomeAsync(EntidadeAcessoAlterarNome entidadeAcessoAlterar)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("EntidadeAcesso/ConfirmarAlteracaoNome/");

                var retorno = await _servico.EnviarEmailAlteracaoNomeAsync(entidadeAcessoAlterar, urlBase);

                return Sucesso("Um e-mail de confirma��o foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarAlteracaoNome/{token}")]
        public async Task<ContentResult> ConfirmarAlteracaoNomeAsync(string token)
        {
            try
            {
                var retorno = await _servico.AlterarNomeAsync(token);
                if (retorno is null)
                    return MensagemHtml("Falha", "N�o encontrada", "N�o foi encontrada uma entidade com este endere�o de e-mail");

                return MensagemHtml("Confirma��o", "Confirma��o de altera��o", "A altera��o foi realizada com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Ser� necess�rio solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicita��o", $"Por favor, entre em contato com o respons�vel pelo sistema. Erro: {e.Message}");
            }
        }

        [HttpPut("AlterarEmail")]
        public async Task<ActionResult> AlterarEmailAsync(EntidadeAcessoAlterarEmail entidadeAcessoAlterarEmail)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("EntidadeAcesso/ConfirmarAlteracaoEmail/");

                var retorno = await _servico.EnviarEmailAlteracaoEmailAsync(entidadeAcessoAlterarEmail, urlBase);

                return Sucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarAlteracaoEmail/{token}")]
        public async Task<ContentResult> ConfirmarAlteracaoEmailAsync(string token)
        {
            try
            {
                var retorno = await _servico.AlterarEmailAsync(token);
                if (retorno is null)
                    return MensagemHtml("Falha", "N�o encontrada", "N�o foi encontrada uma entidade com este endere�o de e-mail");

                return MensagemHtml("Confirma��o", "Confirma��o de altera��o", "A altera��o foi realizada com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Ser� necess�rio solicitar um novo link");
            }
            catch (Exception e)
            {
                return MensagemHtml("Falha", "Falha ao completar a solicita��o", $"Por favor, entre em contato com o respons�vel pelo sistema. Erro: {e.Message}");
            }
        }

        [HttpDelete("Excluir")]
        public async Task<ActionResult> ExcluirAsync(EntidadeAcessoExcluir entidadeAcessoExcluir)
        {
            try
            {
                var excluida = await _servico.ExcluirAsync(entidadeAcessoExcluir);
                if (!excluida)
                    return NotFound("A chave n�o foi encontrada");

                return Sucesso("Chave exclu�da com sucesso");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
