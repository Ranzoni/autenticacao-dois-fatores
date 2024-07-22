using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Core.Entidades
{
    public class EntidadeAcessoTeste
    {
        private readonly Faker _faker = new();

        [Fact]
        internal void DeveCriarEntidadeAcesso()
        {
            var chaveCriptografia = _faker.Random.AlphaNumeric(16);
            Environment.SetEnvironmentVariable("ENCRYPT_KEY", chaveCriptografia);
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;

            var entidadeAcesso = new EntidadeAcesso(nome, email);

            Assert.NotNull(entidadeAcesso);
            Assert.Equal(nome, entidadeAcesso.Nome);
            Assert.Equal(email, entidadeAcesso.Email);
            Assert.NotEmpty(entidadeAcesso.Chave);
            Assert.False(entidadeAcesso.Ativo);
        }

        [Fact]
        internal void NaoDeveCriarEntidadeAcessoQuandoNaoHaChaveCriptografia()
        {
            Environment.SetEnvironmentVariable("ENCRYPT_KEY", null);

            var chaveCriptografia = _faker.Random.AlphaNumeric(16);
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;

            var exception = Assert.Throws<CriptografiaException>(() => new EntidadeAcesso(nome, email));

            Assert.Equal(NotificacoesCriptografia.ChaveNaoEncontrada.Descricao(), exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void NaoDeveCriarEntidadeAcessoQuandoNomeEhInvalido(string nomeInvalido)
        {
            var email = _faker.Person.Email;

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(nomeInvalido, email));

            Assert.Equal(NotificacoesEntidadeAcesso.NomeInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal void NaoDeveCriarEntidadeAcessoQuandoEmailEhInvalido(string emailInvalido)
        {
            var nome = _faker.Company.CompanyName();

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(nome, emailInvalido));

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
        }

        [Fact]
        internal void DeveCriarEntidadeAcessoCompleta()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(8);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = _faker.Random.Bool();

            var entidadeAcesso = new EntidadeAcesso(id, nome, chave, email, dataCadastro, dataUltimoAcesso, ativo);

            Assert.NotNull(entidadeAcesso);
            Assert.Equal(id, entidadeAcesso.Id);
            Assert.Equal(nome, entidadeAcesso.Nome);
            Assert.Equal(chave, entidadeAcesso.Chave);
            Assert.Equal(email, entidadeAcesso.Email);
        }

        [Fact]
        internal void NaoDeveCriarEntidadeAcessoCompletaQuandoIdEhInvalido()
        {
            var idInvalido = 0;
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(8);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var ativo = _faker.Random.Bool();

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(idInvalido, nome, chave, email, dataCadastro, null, ativo));

            Assert.Equal(NotificacoesEntidadeAcesso.IdInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void NaoDeveCriarEntidadeAcessoCompletaQuandoNomeEhInvalido(string nomeInvalido)
        {
            var id = _faker.Random.Int(1);
            var chave = _faker.Random.AlphaNumeric(8);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var ativo = _faker.Random.Bool();

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(id, nomeInvalido, chave, email, dataCadastro, null, ativo));

            Assert.Equal(NotificacoesEntidadeAcesso.NomeInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal void NaoDeveCriarEntidadeAcessoCompletaQuandoChaveEhInvalida(string chaveInvalida)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var ativo = _faker.Random.Bool();

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(id, nome, chaveInvalida, email, dataCadastro, null, ativo));

            Assert.Equal(NotificacoesEntidadeAcesso.ChaveInvalida.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal void NaoDeveCriarEntidadeAcessoCompletaQuandoEmailEhInvalido(string emailInvalido)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(8);
            var dataCadastro = _faker.Date.Past();
            var ativo = _faker.Random.Bool();

            var excecao = Assert.Throws<EntidadeAcessoException>(() => new EntidadeAcesso(id, nome, chave, emailInvalido, dataCadastro, null, ativo));

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
        }

        [Fact]
        internal void DeveRetornarChaveAcessoSemCriptografia()
        {
            var chaveCriptografia = _faker.Random.AlphaNumeric(16);
            Environment.SetEnvironmentVariable("ENCRYPT_KEY", chaveCriptografia);
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;

            var entidadeAcesso = new EntidadeAcesso(nome, email);
            var chaveSemCriptografia = entidadeAcesso.RetornarChaveSemCriptografia();

            Assert.NotNull(chaveSemCriptografia);
            Assert.Equal(8, chaveSemCriptografia.Length);
            Assert.NotEqual(entidadeAcesso.Chave, chaveSemCriptografia);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        internal void DeveAlterarEntidadeAcessoParaAtivo(bool valorAtual, bool novoValor)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(8);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = valorAtual;

            var entidadeAcesso = new EntidadeAcesso(id, nome, chave, email, dataCadastro, dataUltimoAcesso, ativo);
            entidadeAcesso.Ativar(novoValor);

            Assert.Equal(novoValor, entidadeAcesso.Ativo);
        }
    }
}
