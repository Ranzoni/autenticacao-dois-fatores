using AutenticacaoDoisFatores.Core.Entidades.Base;
using AutenticacaoDoisFatores.Core.Validadores;

namespace AutenticacaoDoisFatores.Core.Entidades
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime? DataAlteracao { get; private set; }
        public DateTime? DataUltimoAcesso { get; private set; }
        public EntidadeAcesso? EntidadeAcesso { get; private set; }

        public Usuario(string nome, string email, string senha, EntidadeAcesso entidadeAcesso)
        {
            UsuarioValidador.ValidarNovoUsuario(nome, email, senha, entidadeAcesso);

            Nome = nome;
            Email = email;
            Senha = senha;
            DataCadastro = DateTime.UtcNow;
            EntidadeAcesso = entidadeAcesso;
        }

        public Usuario(int id, string nome, string email, string senha, DateTime dataCadastro, DateTime? dataAlteracao, DateTime? dataUltimoAcesso)
        {
            UsuarioValidador.ValidarUsuarioCompleto(id, nome, email, senha);

            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            DataCadastro = dataCadastro;
            DataAlteracao = dataAlteracao;
            DataUltimoAcesso = dataUltimoAcesso;
        }
    }
}
