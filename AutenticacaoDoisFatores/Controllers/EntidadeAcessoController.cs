using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadeAcessoController(IEntidadeAcessoServico servico, INotificador notificador) : BaseController(notificador)
    {
        private readonly IEntidadeAcessoServico _servico = servico;

        [HttpPost(Name = "CadastrarEntidadeAcesso")]
        public async Task<ActionResult<EntidadeAcessoCadastrada?>> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            try
            {
                var entidadeAcessoCadastrada = await _servico.CadastrarAsync(entidadeAcessoCadastrar);

                return CriadoComSucesso(entidadeAcessoCadastrada);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(Name = "ReenviarChaveAcesso")]
        public async Task<ActionResult> ReenviarChaveAcessoAsync(ReenviarChaveAcesso reenviarChaveAcesso)
        {
            try
            {
                await _servico.ReenviarChaveAcessoAsync(reenviarChaveAcesso);

                return AlteradoComSucesso();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
