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
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;

            var entidadeAcesso = new EntidadeAcesso(nome, email);

            Assert.NotNull(entidadeAcesso);
            Assert.Equal(nome, entidadeAcesso.Nome);
            Assert.Equal(email, entidadeAcesso.Email);
            Assert.NotEmpty(entidadeAcesso.Chave);
            Assert.False(entidadeAcesso.Ativo);
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
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;

            var entidadeAcesso = new EntidadeAcesso(nome, email);
            var chaveSemCriptografia = "";

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

        [Fact]
        internal void DeveAlterarNomeEntidadeAcesso()
        {
            var nomeAnterior = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nomeAnterior, email);
            var novoNome = $"{_faker.Company.CompanyName()}_NOVO";

            entidadeAcesso.AlterarNome(novoNome);

            Assert.NotEqual(nomeAnterior, entidadeAcesso.Nome);
            Assert.Equal(novoNome, entidadeAcesso.Nome);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void DeveRetornarExcecaoAoTentarAlterarNomeComValorInvalido(string nomeInvalido)
        {
            var nomeAnterior = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nomeAnterior, email);

            var excecao = Assert.Throws<EntidadeAcessoException>(() => entidadeAcesso.AlterarNome(nomeInvalido));

            Assert.Equal(NotificacoesEntidadeAcesso.NomeInvalido.Descricao(), excecao.Message);
            Assert.Equal(nomeAnterior, entidadeAcesso.Nome);
        }

        [Fact]
        internal void DeveAlterarEmailEntidadeAcesso()
        {
            var nome = _faker.Company.CompanyName();
            var emailAnterior = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nome, emailAnterior);
            var novoEmail = $"novo_{_faker.Person.Email}";

            entidadeAcesso.AlterarEmail(novoEmail);

            Assert.NotEqual(nome, entidadeAcesso.Email);
            Assert.Equal(novoEmail, entidadeAcesso.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal void DeveRetornarExcecaoAoTentarAlterarEmailComValorInvalido(string emailInvalido)
        {
            var nome = _faker.Company.CompanyName();
            var emailAnterior = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nome, emailAnterior);

            var excecao = Assert.Throws<EntidadeAcessoException>(() => entidadeAcesso.AlterarEmail(emailInvalido));

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
            Assert.Equal(emailAnterior, entidadeAcesso.Email);
        }
    }
}
