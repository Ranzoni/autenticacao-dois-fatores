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
            var email = _faker.Person.Email;
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.ExisteUsuarioComEmailAsync(email)).ReturnsAsync(existe);

            var retorno = await dominio.ExisteUsuarioComEmailAsync(email);

            Assert.Equal(existe, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.ExisteUsuarioComEmailAsync(email), Times.Once);
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
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var usuario = construtor
                .ComId(id)
                .CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();
            _mock.GetMock<IUsuarioRepositorio>().Setup(r => r.BuscarAsync(id)).ReturnsAsync(usuario);

            var retorno = await dominio.BuscarAsync(id);

            Assert.NotNull(retorno);
            Assert.Equal(usuario, retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExisteUsuarioComId()
        {
            var id = _faker.Random.Int(1);
            var construtor = new UsuarioConstrutor();
            var usuario = construtor
                .ComId(id)
                .CriarCompleto();
            var dominio = _mock.CreateInstance<UsuarioDominio>();

            var retorno = await dominio.BuscarAsync(id);

            Assert.Null(retorno);
            _mock.GetMock<IUsuarioRepositorio>().Verify(r => r.BuscarAsync(id), Times.Once);
        }
    }
}
