using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadeAcessoController(IEntidadeAcessoServico servico, INotificador notificador, IConfiguration config) : BaseController(notificador)
    {
        private readonly IEntidadeAcessoServico _servico = servico;
        private readonly IConfiguration _config = config;

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<EntidadeAcessoResposta?>> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            try
            {
                var urlAplicacao = _config.GetValue<string>("AutenticacaoDoisFatores:UrlBase");
                var urlBase = $"{urlAplicacao}EntidadeAcesso/ConfirmarCadastro/";

                var entidadeAcessoCadastrada = await _servico.CadastrarAsync(entidadeAcessoCadastrar, urlBase);

                return CriadoComSucesso(entidadeAcessoCadastrada);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ReenviarChaveAcesso")]
        public async Task<ActionResult> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso)
        {
            try
            {
                var urlAplicacao = _config.GetValue<string>("AutenticacaoDoisFatores:UrlBase");
                var urlBase = $"{urlAplicacao}EntidadeAcesso/NovaChaveAcesso/";

                await _servico.ReenviarChaveAcessoAsync(reenviarChaveAcesso, urlBase);

                return Sucesso("Um e-mail de recuperação foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("NovaChaveAcesso/{token}")]
        public async Task<ContentResult> NovaChaveAcessoAsync(string token)
        {
            try
            {
                await _servico.AlterarChaveAcessoAsync(token);

                return MensagemHtml("Confirmação", "Confirmação de reenvio de chave", "Foi enviado um novo e-mail com a nova chave de acesso");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", "Por favor, entre em contato com o responsável pelo sistema");
            }
        }

        [HttpGet("ConfirmarCadastro/{token}")]
        public async Task<ContentResult> ConfirmarCadastroAsync(string token)
        {
            try
            {
                await _servico.AtivarCadastroAsync(token);

                return MensagemHtml("Confirmação", "Confirmação de cadastro", "O cadastro foi ativado com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", "Por favor, entre em contato com o responsável pelo sistema");
            }
        }

        [HttpPut("AlterarNome")]
        public async Task<ActionResult> AlterarNomeAsync(EntidadeAcessoAlterar entidadeAcessoAlterar)
        {
            try
            {
                var urlAplicacao = _config.GetValue<string>("AutenticacaoDoisFatores:UrlBase");
                var urlBase = $"{urlAplicacao}EntidadeAcesso/ConfirmarAlteracao/";

                await _servico.EnviarEmailAlteracaoNomeAsync(entidadeAcessoAlterar, urlBase);

                return Sucesso("Um e-mail de confirmação foi enviado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ConfirmarAlteracao/{token}")]
        public async Task<ContentResult> ConfirmarAlteracaoAsync(string token)
        {
            try
            {
                await _servico.AlterarNomeAsync(token);

                return MensagemHtml("Confirmação", "Confirmação de alteração", "A alteração foi realizada com sucesso!");
            }
            catch (SecurityTokenExpiredException)
            {
                return MensagemHtml("Falha", "Link expirado", "Será necessário solicitar um novo link");
            }
            catch
            {
                return MensagemHtml("Falha", "Falha ao completar a solicitação", "Por favor, entre em contato com o responsável pelo sistema");
            }
        }

        [HttpPut("AlterarEmail/{emailAtual}")]
        public async Task<ActionResult> AlterarEmailAsync(string emailAtual, [FromBody] EntidadeAcessoAlterarEmail entidadeAcessoAlterarEmail)
        {
            try
            {
                var retorno = await _servico.AlterarEmailAsync(emailAtual, entidadeAcessoAlterarEmail);
                if (retorno is null)
                    return NaoEncontrado("A entidade de acesso não foi encontrada");

                return Sucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
