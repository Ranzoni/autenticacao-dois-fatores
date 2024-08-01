using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Teste.Construtores;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Core.Entidades
{
    public class UsuarioTeste
    {
        private readonly Faker _faker = new();

        [Fact]
        internal void DeveCriarUsuario()
        {
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var entidadeAcesso = EntidadeAcessoConstrutor.CriarEntidadeAcesso();

            var usuario = new Usuario(nome, email, senha, entidadeAcesso);

            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(senha, usuario.Senha);
            Assert.Equal(entidadeAcesso, usuario.EntidadeAcesso);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void NaoDeveCriarUsuarioComNomeInvalido(string nomeInvalido)
        {
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var entidadeAcesso = EntidadeAcessoConstrutor.CriarEntidadeAcesso();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(nomeInvalido, email, senha, entidadeAcesso));

            Assert.Equal(NotificacoesUsuario.NomeInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal void NaoDeveCriarUsuarioComEmailInvalido(string emailInvalido)
        {
            var nome = _faker.Person.FullName;
            var senha = _faker.Random.AlphaNumeric(10);
            var entidadeAcesso = EntidadeAcessoConstrutor.CriarEntidadeAcesso();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(nome, emailInvalido, senha, entidadeAcesso));

            Assert.Equal(NotificacoesUsuario.EmailInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal void NaoDeveCriarUsuarioComSenhaInvalida(string senhaInvalida)
        {
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var entidadeAcesso = EntidadeAcessoConstrutor.CriarEntidadeAcesso();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(nome, email, senhaInvalida, entidadeAcesso));

            Assert.Equal(NotificacoesUsuario.SenhaInvalida.Descricao(), excecao.Message);
        }

        [Fact]
        internal void NaoDeveCriarUsuarioQuandoEntidadeAcessoEhNula()
        {
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(nome, email, senha, null));

            Assert.Equal(NotificacoesUsuario.ChaveAcessoNaoInformada.Descricao(), excecao.Message);
        }

        [Fact]
        internal void DeveCriarUsuarioCompleto()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var dataCadastro = _faker.Date.Past();
            var dataAlteracao = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Past();

            var usuario = new Usuario(id, nome, email, senha, dataCadastro, dataAlteracao, dataUltimoAcesso);

            Assert.Equal(id, usuario.Id);
            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(senha, usuario.Senha);
            Assert.Equal(dataCadastro, usuario.DataCadastro);
            Assert.Equal(dataAlteracao, usuario.DataAlteracao);
            Assert.Equal(dataUltimoAcesso, usuario.DataUltimoAcesso);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        internal void NaoDeveCriarUsuarioCompletoComIdInvalido(int idInvalido)
        {
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var dataCadastro = _faker.Date.Past();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(idInvalido, nome, email, senha, dataCadastro, null, null));

            Assert.Equal(NotificacoesUsuario.IdInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void NaoDeveCriarUsuarioCompletoComNomeInvalido(string nomeInvalido)
        {
            var id = _faker.Random.Int(1);
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var dataCadastro = _faker.Date.Past();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(id, nomeInvalido, email, senha, dataCadastro, null, null));

            Assert.Equal(NotificacoesUsuario.NomeInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal void NaoDeveCriarUsuarioCompletoComEmailInvalido(string emailInvalido)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Person.FullName;
            var senha = _faker.Random.AlphaNumeric(10);
            var dataCadastro = _faker.Date.Past();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(id, nome, emailInvalido, senha, dataCadastro, null, null));

            Assert.Equal(NotificacoesUsuario.EmailInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal void NaoDeveCriarUsuarioCompletoComSenhaInvalida(string senhaInvalida)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();

            var excecao = Assert.Throws<UsuarioException>(() => new Usuario(id, nome, email, senhaInvalida, dataCadastro, null, null));

            Assert.Equal(NotificacoesUsuario.SenhaInvalida.Descricao(), excecao.Message);
        }
    }
}
