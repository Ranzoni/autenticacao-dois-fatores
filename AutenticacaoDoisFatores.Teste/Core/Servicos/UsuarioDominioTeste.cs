using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos;
using AutenticacaoDoisFatores.Teste.Construtores;
using Bogus;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Core.Servicos
{
    public class UsuarioDominioTeste
    {
        private readonly Faker _faker = new();
        private readonly AutoMocker _mock = new();

        [Fact]
        internal async Task DeveCriarUsuario()
        {
            var construtor = new UsuarioConstrutor();
            var usuario = construtor.Criar();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            await dominio.CadastrarAsync(usuario);

            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.CadastrarAsync(usuario), Times.Once);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal async Task DeveBuscarSeExisteUsuarioComEmail(bool existe)
        {
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.ExisteUsuarioComEmailAsync(email, chave)).ReturnsAsync(existe);

            var retorno = await dominio.ExisteUsuarioComEmailAsync(email, chave);

            Assert.Equal(existe, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.ExisteUsuarioComEmailAsync(email, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveAlterarUsuario()
        {
            var construtor = new UsuarioConstrutor();
            var usuario = construtor.CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            await dominio.AlterarAsync(usuario);

            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.Alterar(usuario), Times.Once);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarUsuarioPorId()
        {
            var chave = Guid.NewGuid();
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var usuario = construtor
                .ComId(id)
                .CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.BuscarAsync(id, chave)).ReturnsAsync(usuario);

            var retorno = await dominio.BuscarAsync(id, chave);

            Assert.NotNull(retorno);
            Assert.Equal(usuario, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExisteUsuarioComId()
        {
            var chave = Guid.NewGuid();
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            var retorno = await dominio.BuscarAsync(id, chave);

            Assert.Null(retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarUsuarioNaoAtivo()
        {
            var chave = Guid.NewGuid();
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var usuario = construtor
                .ComId(id)
                .ComAtivo(false)
                .CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.BuscarNaoAtivoAsync(id, chave)).ReturnsAsync(usuario);

            var retorno = await dominio.BuscarNaoAtivoAsync(id, chave);

            Assert.NotNull(retorno);
            Assert.Equal(usuario, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarNaoAtivoAsync(id, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExisteUsuarioNaoAtivo()
        {
            var chave = Guid.NewGuid();
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            var retorno = await dominio.BuscarNaoAtivoAsync(id, chave);

            Assert.Null(retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarNaoAtivoAsync(id, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarUsuarioPorEmail()
        {
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;
            var construtor = new UsuarioConstrutor();
            var usuario = construtor
                .ComEmail(email)
                .CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.BuscarPorEmailAsync(email, chave)).ReturnsAsync(usuario);

            var retorno = await dominio.BuscarPorEmailAsync(email, chave);

            Assert.NotNull(retorno);
            Assert.Equal(usuario, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarPorEmailAsync(email, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExisteUsuarioComEmail()
        {
            var chave = Guid.NewGuid();
            var email = _faker.Person.Email;
            var construtor = new UsuarioConstrutor();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            var retorno = await dominio.BuscarPorEmailAsync(email, chave);

            Assert.Null(retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarPorEmailAsync(email, chave), Times.Once);
        }

        [Fact]
        internal async Task DeveExcluirUsuario()
        {
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            var id = _faker.Random.Int(1);
            var chave = Guid.NewGuid();
            var usuario = new UsuarioConstrutor()
                .ComId(id)
                .CriarCompleto();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.BuscarAsync(id, chave)).ReturnsAsync(usuario);

            await dominio.ExcluirAsync(id, chave);

            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id, chave), Times.Once);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.Excluir(usuario), Times.Once);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarExcecaoAoExcluirUsuarioQueNaoExiste()
        {
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            var id = _faker.Random.Int(1);
            var chave = Guid.NewGuid();

            var excecao = await Assert.ThrowsAsync<UsuarioException>(() => dominio.ExcluirAsync(id, chave));

            Assert.Equal(NotificacoesUsuario.NaoEncontrado.Descricao(), excecao.Message);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id, chave), Times.Once);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.Excluir(It.IsAny<Usuario>()), Times.Never);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }
    }
}
