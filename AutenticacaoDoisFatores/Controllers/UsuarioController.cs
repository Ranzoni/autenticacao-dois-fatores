using AutenticacaoDoisFatores.Controllers.Base;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioControllerUsuarioController(INotificadorServico notificador, IConfiguration configuration, IUsuarioServico _servico) : BaseController(notificador, configuration)
    {
        [HttpPost("Cadastrar")]
        public async Task<ActionResult<UsuarioResposta?>> CadastrarAsync(UsuarioCadastrar usuarioCadastrar)
        {
            try
            {
                var urlBase = RetornarUrlFormatada("Usuario/ConfirmarCadastro/");

                var retorno = await _servico.CadastrarAsync(usuarioCadastrar, "");
                if (retorno is null)
                    return NaoEncontrado("A chave de acesso não foi encontrada");

                return CriadoComSucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
