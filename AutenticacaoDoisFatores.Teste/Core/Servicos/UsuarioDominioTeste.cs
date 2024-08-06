using AutenticacaoDoisFatores.Core.Entidades;
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
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(32);
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor
                .CriarEntidadeAcesso();
            var usuario = new Usuario(nome, email, senha, entidadeAcesso);
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
    }
}
