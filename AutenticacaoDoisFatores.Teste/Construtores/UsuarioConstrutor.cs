using AutenticacaoDoisFatores.Core.Entidades;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Construtores
{
    internal class UsuarioConstrutor
    {
        private readonly Faker _faker = new();
        private int _id;
        private string _nome;
        private string _email;
        private string _senha;
        private DateTime _dataCadastro;
        private DateTime? _dataAlteracao;
        private DateTime? _dataUltimoAcesso;
        private bool _ativo;
        private EntidadeAcesso _entidadeAcesso;

        public UsuarioConstrutor()
        {
            _id = _faker.Random.Int(1);
            _nome = _faker.Person.FullName;
            _email = _faker.Person.Email;
            _senha = _faker.Random.AlphaNumeric(10);
            _dataCadastro = _faker.Date.Past();
            var construtor = new EntidadeAcessoConstrutor();
            _entidadeAcesso = construtor.CriarEntidadeAcessoCompleta();
        }

        public UsuarioConstrutor ComId(int entidadeAcesso)
        {
            _id = entidadeAcesso;

            return this;
        }

        public UsuarioConstrutor ComNome(string nome)
        {
            _nome = nome;

            return this;
        }

        public UsuarioConstrutor ComEmail(string email)
        {
            _email = email;

            return this;
        }

        public UsuarioConstrutor ComSenha(string senha)
        {
            _senha = senha;

            return this;
        }

        public UsuarioConstrutor ComDataCadastro(DateTime dataCadastro)
        {
            _dataCadastro = dataCadastro;

            return this;
        }

        public UsuarioConstrutor ComDataAlteracao(DateTime dataAlteracao)
        {
            _dataAlteracao = dataAlteracao;

            return this;
        }

        public UsuarioConstrutor ComDataUltimoAcesso(DateTime dataUltimoAcesso)
        {
            _dataUltimoAcesso = dataUltimoAcesso;

            return this;
        }

        public UsuarioConstrutor ComAtivo(bool ativo)
        {
            _ativo = ativo;

            return this;
        }

        public UsuarioConstrutor ComEntidadeAcesso(EntidadeAcesso entidadeAcesso)
        {
            _entidadeAcesso = entidadeAcesso;

            return this;
        }

        public Usuario Criar()
        {
            var usuario = new Usuario(_nome, _email, _senha, _entidadeAcesso);

            return usuario;
        }

        public Usuario CriarCompleto()
        {
            var usuario = new Usuario(_id, _nome, _email, _senha, _dataCadastro, _dataAlteracao, _dataUltimoAcesso, _ativo);

            return usuario;
        }
    }
}
