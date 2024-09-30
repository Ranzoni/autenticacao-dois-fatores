using AutenticacaoDoisFatores.Controllers.Base;
using AutenticacaoDoisFatores.Servico.DTO.Autenticacao;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using Mensageiro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController(INotificador _notificador, IConfiguration _configuration, IAutenticacaoServico _servico) : BaseController(_notificador, _configuration)
    {
        [HttpPost("Autenticar")]
        public async Task<ActionResult<UsuarioAutenticado?>> AutenticarAsync(UsuarioAutenticar usuarioAutenticar)
        {
            try
            {
                var retorno = await _servico.AutenticarAsync(usuarioAutenticar);

                return Sucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("Inativar/{id}")]
        public async Task<ActionResult> InativarAsync(int id)
        {
            try
            {
                var chave = RetornarDadosTokenAutenticacao().chave;
                var retorno = await _servico.InativarAsync(id, chave);

                return Sucesso("Usuário inativado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
