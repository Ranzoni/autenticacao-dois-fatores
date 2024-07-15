using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadeAcessoController(IEntidadeAcessoServico servico) : ControllerBase
    {
        private readonly IEntidadeAcessoServico _servico = servico;

        [HttpPost(Name = "CadastrarEntidadeAcesso")]
        public async Task<ActionResult<EntidadeAcessoCadastrada>> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            try
            {
                var entidadeAcessoCadastrada = await _servico.CadastrarAsync(entidadeAcessoCadastrar);

                return StatusCode(201, entidadeAcessoCadastrada);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
