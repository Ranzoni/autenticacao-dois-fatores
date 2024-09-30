using AutenticacaoDoisFatores.Core.Entidades;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Construtores
{
    internal class EntidadeAcessoConstrutor
    {
        private readonly Faker _faker = new();
        private int _id;
        private string _nome;
        private Guid _chave;
        private string _email;
        private DateTime _dataCadastro;
        private DateTime? _dataUltimoAcesso;
        private bool _ativo;

        internal EntidadeAcessoConstrutor()
        {
            _id = _faker.Random.Int(1);
            _nome = _faker.Company.CompanyName();
            _chave = Guid.NewGuid();
            _email = _faker.Person.Email;
            _dataCadastro = _faker.Date.Past();
        }

        internal EntidadeAcessoConstrutor ComId(int id)
        {
            _id = id;

            return this;
        }

        internal EntidadeAcessoConstrutor ComNome(string nome)
        {
            _nome = nome;

            return this;
        }

        internal EntidadeAcessoConstrutor ComChave(Guid chave)
        {
            _chave = chave;

            return this;
        }

        internal EntidadeAcessoConstrutor ComEmail(string email)
        {
            _email = email;

            return this;
        }

        internal EntidadeAcessoConstrutor ComDataCadastro(DateTime dataCadastro)
        {
            _dataCadastro = dataCadastro;

            return this;
        }

        internal EntidadeAcessoConstrutor ComDataUltimoAcesso(DateTime dataUltimoAcesso)
        {
            _dataUltimoAcesso = dataUltimoAcesso;

            return this;
        }

        internal EntidadeAcessoConstrutor ComAtivo(bool ativo)
        {
            _ativo = ativo;

            return this;
        }

        internal EntidadeAcesso CriarEntidadeAcesso()
        {
            var entidadeAcesso = new EntidadeAcesso(_nome, _chave, _email);

            return entidadeAcesso;
        }

        internal EntidadeAcesso CriarEntidadeAcessoCompleta()
        {
            var entidadeAcesso = new EntidadeAcesso(_id, _nome, _chave, _email, _dataCadastro, _dataUltimoAcesso, _ativo);

            return entidadeAcesso;
        }
    }
}
