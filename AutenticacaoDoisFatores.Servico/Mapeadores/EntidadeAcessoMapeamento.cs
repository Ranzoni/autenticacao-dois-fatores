using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico.Mapeadores
{
    public class EntidadeAcessoMapeamento : Profile
    {
        public EntidadeAcessoMapeamento()
        {
            CreateMap<EntidadeAcessoCadastrar, EntidadeAcesso>();
            CreateMap<EntidadeAcesso, EntidadeAcessoCadastrada>();
        }
    }
}
