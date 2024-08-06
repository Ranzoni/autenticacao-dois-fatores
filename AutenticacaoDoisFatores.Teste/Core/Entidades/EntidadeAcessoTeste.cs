using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Teste.Construtores;
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
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;

            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComNome(nome)
                .ComChave(chave)
                .ComEmail(email)
                .CriarEntidadeAcesso();

            Assert.NotNull(entidadeAcesso);
            Assert.Equal(nome, entidadeAcesso.Nome);
            Assert.Equal(chave, entidadeAcesso.Chave);
            Assert.Equal(email, entidadeAcesso.Email);
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
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;

            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                var construtor = new EntidadeAcessoConstrutor();
                var entidadeAcesso = construtor
                    .ComNome(nomeInvalido)
                    .ComChave(chave)
                    .ComEmail(email)
                    .CriarEntidadeAcesso();
            });

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
            var chave = Guid.NewGuid();

            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                var construtor = new EntidadeAcessoConstrutor();
                var entidadeAcesso = construtor
                    .ComNome(nome)
                    .ComChave(chave)
                    .ComEmail(emailInvalido)
                    .CriarEntidadeAcesso();
            });

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
        }

        [Fact]
        internal void DeveCriarEntidadeAcessoCompleta()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = _faker.Random.Bool();

            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComId(id)
                .ComNome(nome)
                .ComChave(chave)
                .ComEmail(email)
                .ComDataCadastro(dataCadastro)
                .ComDataUltimoAcesso(dataUltimoAcesso)
                .ComAtivo(ativo)
                .CriarEntidadeAcessoCompleta();

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

            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                var construtor = new EntidadeAcessoConstrutor();
                var entidadeAcesso = construtor
                    .ComId(idInvalido)
                    .CriarEntidadeAcessoCompleta();
            });

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
            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                var construtor = new EntidadeAcessoConstrutor();
                var entidadeAcesso = construtor
                    .ComNome(nomeInvalido)
                    .CriarEntidadeAcessoCompleta();
            });

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
        internal void NaoDeveCriarEntidadeAcessoCompletaQuandoEmailEhInvalido(string emailInvalido)
        {
            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                var construtor = new EntidadeAcessoConstrutor();
                var entidadeAcesso = construtor
                    .ComEmail(emailInvalido)
                    .CriarEntidadeAcessoCompleta();
            });

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        internal void DeveAlterarEntidadeAcessoParaAtivo(bool valorAtual, bool novoValor)
        {
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComAtivo(valorAtual)
                .CriarEntidadeAcessoCompleta();

            entidadeAcesso.Ativar(novoValor);

            Assert.NotEqual(valorAtual, entidadeAcesso.Ativo);
            Assert.Equal(novoValor, entidadeAcesso.Ativo);
        }

        [Fact]
        internal void DeveAlterarNomeEntidadeAcesso()
        {
            var nomeAnterior = _faker.Company.CompanyName();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComNome(nomeAnterior)
                .CriarEntidadeAcessoCompleta();
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
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComNome(nomeAnterior)
                .CriarEntidadeAcessoCompleta();

            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                entidadeAcesso.AlterarNome(nomeInvalido);
            });

            Assert.Equal(NotificacoesEntidadeAcesso.NomeInvalido.Descricao(), excecao.Message);
            Assert.Equal(nomeAnterior, entidadeAcesso.Nome);
        }

        [Fact]
        internal void DeveAlterarEmailEntidadeAcesso()
        {
            var emailAnterior = _faker.Person.Email;
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComEmail(emailAnterior)
                .CriarEntidadeAcessoCompleta();
            var novoEmail = $"novo_{_faker.Person.Email}";

            entidadeAcesso.AlterarEmail(novoEmail);

            Assert.NotEqual(emailAnterior, entidadeAcesso.Email);
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
            var chave = Guid.NewGuid();
            var emailAnterior = _faker.Person.Email;
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComEmail(emailAnterior)
                .CriarEntidadeAcessoCompleta();

            var excecao = Assert.Throws<EntidadeAcessoException>(() =>
            {
                entidadeAcesso.AlterarEmail(emailInvalido);
            });

            Assert.Equal(NotificacoesEntidadeAcesso.EmailInvalido.Descricao(), excecao.Message);
            Assert.Equal(emailAnterior, entidadeAcesso.Email);
        }

        [Fact]
        internal void DeveAlterarChaveEntidadeAcesso()
        {
            var chaveAnterior = Guid.NewGuid();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .ComChave(chaveAnterior)
                .CriarEntidadeAcessoCompleta();
            var novaChave = Guid.NewGuid();

            entidadeAcesso.AlterarChave(novaChave);

            Assert.NotEqual(chaveAnterior, entidadeAcesso.Chave);
            Assert.Equal(novaChave, entidadeAcesso.Chave);
        }
    }
}
