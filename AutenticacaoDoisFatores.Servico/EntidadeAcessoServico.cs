using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Interfaces;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico
{
    public class EntidadeAcessoServico(IEntidadeAcessoDominio dominio, IMapper mapeador) : IEntidadeAcessoServico
    {
        private readonly IEntidadeAcessoDominio _dominio = dominio;
        private readonly IMapper _mapeador = mapeador;

        public async Task<EntidadeAcessoCadastrada> CadastrarAsync(EntidadeAcessoCadastrar entidadeAcessoCadastrar)
        {
            var entidadeAcessoMapeada = _mapeador.Map<EntidadeAcesso>(entidadeAcessoCadastrar);

            var entidadeAcesso = await _dominio.CadastrarAsync(entidadeAcessoMapeada);

            var entidadeAcesssoCadastrada = _mapeador.Map<EntidadeAcessoCadastrada>(entidadeAcesso);

            return entidadeAcesssoCadastrada;
        }
    }
}
