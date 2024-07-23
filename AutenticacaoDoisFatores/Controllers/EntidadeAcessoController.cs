using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadeAcessoController(IEntidadeAcessoServico servico, INotificador notificador, IConfiguration config) : BaseController(notificador)
    {
        private readonly IEntidadeAcessoServico _servico = servico;
        private readonly IConfiguration _config = config;

        [HttpPost("CadastrarEntidadeAcesso")]
        public async Task<ActionResult<EntidadeAcessoCadastrada?>> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
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

        [HttpPost("ReenviarChaveAcesso")]
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
        public async Task<ActionResult> NovaChaveAcessoAsync(string token)
        {
            try
            {
                await _servico.AlterarChaveAcessoAsync(token);

                return Sucesso("Foi enviado um novo e-mail com a nova chave de acesso");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
