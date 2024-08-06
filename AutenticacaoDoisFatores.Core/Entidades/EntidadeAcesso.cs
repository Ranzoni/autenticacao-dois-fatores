using AutenticacaoDoisFatores.Core.Entidades.Base;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Validadores;

namespace AutenticacaoDoisFatores.Core.Entidades
{
    public class EntidadeAcesso: EntidadeBase
    {
        public string Nome { get; private set; }
        public Guid Chave { get; private set; }
        public string Email { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime? DataUltimoAcesso { get; private set; }
        public bool Ativo { get; private set; }

        public EntidadeAcesso(string nome, Guid chave, string email)
        {
            EntidadeAcessoValidador.ValidarNovaEntidade(nome, chave, email);

            Nome = nome;
            Chave = chave;
            Email = email;
            DataCadastro = DateTime.UtcNow;
            Ativo = false;
        }

        public EntidadeAcesso(int id, string nome, Guid chave, string email, DateTime dataCadastro, DateTime? dataUltimoAcesso, bool ativo)
        {
            EntidadeAcessoValidador.ValidarEntidadeCompleta(id, nome, chave, email);

            Id = id;
            Nome = nome;
            Chave = chave;
            Email = email;
            DataCadastro = dataCadastro;
            DataUltimoAcesso = dataUltimoAcesso;
            Ativo = ativo;
        }

        public void AlterarNome(string nome)
        {
            if (!EntidadeAcessoValidador.NomeEhValido(nome))
                EntidadeAcessoException.NomeInvalido();

            Nome = nome;
        }

        public void AlterarEmail(string email)
        {
            if (!EntidadeAcessoValidador.EmailEhValido(email))
                EntidadeAcessoException.EmailInvalido();

            Email = email;
        }
        
        public void AlterarChave(Guid chave)
        {
            Chave = chave;
        }

        public void Ativar(bool valor)
        {
            Ativo = valor;
        }
    }
}
