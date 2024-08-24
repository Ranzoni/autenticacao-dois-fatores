using AutenticacaoDoisFatores.Controllers.Base;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using Mensageiro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoDoisFatores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController(INotificador notificador, IConfiguration configuration, IUsuarioServico _servico) : BaseController(notificador, configuration)
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
                var retorno = await _servico.AtivarAsync(token);
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

        [HttpPut("Alterar/{id}")]
        public async Task<ActionResult<UsuarioResposta?>> AlterarAsync(int id, UsuarioAlterar usuarioAlterar)
        {
            try
            {
                var retorno = await _servico.AlterarAsync(id, usuarioAlterar);

                return Sucesso(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
