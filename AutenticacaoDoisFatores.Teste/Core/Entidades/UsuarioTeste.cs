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
            var entidadeAcesso = new EntidadeAcessoConstrutor()
                .CriarEntidadeAcesso();

            var usuario = new UsuarioConstrutor()
                .ComNome(nome)
                .ComEmail(email)
                .ComSenha(senha)
                .ComEntidadeAcesso(entidadeAcesso)
                .Criar();

            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(senha, usuario.Senha);
            Assert.False(usuario.Ativo);
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
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComNome(nomeInvalido)
                .Criar());

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
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComEmail(emailInvalido)
                .Criar());

            Assert.Equal(NotificacoesUsuario.EmailInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal void NaoDeveCriarUsuarioComSenhaInvalida(string senhaInvalida)
        {
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComSenha(senhaInvalida)
                .Criar());

            Assert.Equal(NotificacoesUsuario.SenhaInvalida.Descricao(), excecao.Message);
        }

        [Fact]
        internal void NaoDeveCriarUsuarioQuandoEntidadeAcessoEhNula()
        {
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComEntidadeAcesso(null)
                .Criar());

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
            var ativo = _faker.Random.Bool();

            var usuario = new UsuarioConstrutor()
                .ComId(id)
                .ComNome(nome)
                .ComEmail(email)
                .ComSenha(senha)
                .ComDataCadastro(dataCadastro)
                .ComDataAlteracao(dataAlteracao)
                .ComDataUltimoAcesso(dataUltimoAcesso)
                .ComAtivo(ativo)
                .CriarCompleto();

            Assert.Equal(id, usuario.Id);
            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(senha, usuario.Senha);
            Assert.Equal(dataCadastro, usuario.DataCadastro);
            Assert.Equal(dataAlteracao, usuario.DataAlteracao);
            Assert.Equal(dataUltimoAcesso, usuario.DataUltimoAcesso);
            Assert.Equal(ativo, usuario.Ativo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        internal void NaoDeveCriarUsuarioCompletoComIdInvalido(int idInvalido)
        {
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComId(idInvalido)
                .CriarCompleto());

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
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComNome(nomeInvalido)
                .CriarCompleto());

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
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComEmail(emailInvalido)
                .CriarCompleto());

            Assert.Equal(NotificacoesUsuario.EmailInvalido.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal void NaoDeveCriarUsuarioCompletoComSenhaInvalida(string senhaInvalida)
        {
            var excecao = Assert.Throws<UsuarioException>(() =>
                new UsuarioConstrutor()
                .ComSenha(senhaInvalida)
                .CriarCompleto());

            Assert.Equal(NotificacoesUsuario.SenhaInvalida.Descricao(), excecao.Message);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        internal void DeveAlterarAtivo(bool valorAtual, bool novoValor)
        {
            var usuario = new UsuarioConstrutor()
                .ComAtivo(valorAtual)
                .CriarCompleto();

            usuario.Ativar(novoValor);

            Assert.Equal(novoValor, usuario.Ativo);
        }

        [Fact]
        internal void DeveAlterarNome()
        {
            var nomeAtual = _faker.Person.FullName;
            var usuario = new UsuarioConstrutor()
                .ComNome(nomeAtual)
                .Criar();
            var novoNome = $"{_faker.Person.FullName} Silva";

            usuario.AlterarNome(novoNome);

            Assert.Equal(novoNome, usuario.Nome);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal void NaoDeveAlterarNomeInvalido(string nomeInvalido)
        {
            var nomeAtual = _faker.Person.FullName;
            var usuario = new UsuarioConstrutor()
                .ComNome(nomeAtual)
                .Criar();

            var excecao = Assert.Throws<UsuarioException>(() => usuario.AlterarNome(nomeInvalido));

            Assert.Equal(NotificacoesUsuario.NomeInvalido.Descricao(), excecao.Message);
        }
    }
}
