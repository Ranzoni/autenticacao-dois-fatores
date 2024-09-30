using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico.Mapeadores
{
    public class UsuarioMapeamento : Profile
    {
        public UsuarioMapeamento()
        {
            CreateMap<Usuario, UsuarioResposta>();
        }
    }
}
