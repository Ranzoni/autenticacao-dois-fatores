using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos;
using Bogus;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Core.Servicos
{
    public class EntidadeAcessoDominioTeste
    {
        private readonly AutoMocker _mock = new();
        private readonly Faker _faker = new();

        public EntidadeAcessoDominioTeste()
        {
            var chaveCriptografia = _faker.Random.AlphaNumeric(16);
            Environment.SetEnvironmentVariable("ENCRYPT_KEY", chaveCriptografia);
        }

        [Fact]
        internal async Task DeveCriarEntidadeAcesso()
        {
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nome, email);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var retorno = await dominio.CadastrarAsync(entidadeAcesso);

            Assert.NotNull(retorno);
            Assert.False(retorno.Ativo);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.CadastrarAsync(entidadeAcesso), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal async Task DeveRetornarEntidadeSeExistente(bool retornoRep)
        {
            var email = _faker.Person.Email;
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.ExisteEntidadeComEmailAsync(email)).ReturnsAsync(retornoRep);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var retorno = await dominio.ExisteEntidadeComEmailAsync(email);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.ExisteEntidadeComEmailAsync(email), Times.Once);
            Assert.Equal(retornoRep, retorno);
        }

        [Fact]
        internal async Task DeveAlterarChaveEntidadeAcesso()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(32);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = _faker.Random.Bool();
            var entidadeAcessoCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, dataUltimoAcesso, ativo);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.BuscarPorEmailAsync(email)).ReturnsAsync(entidadeAcessoCadastrada);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var retorno = await dominio.GerarNovaChaveAsync(email);

            Assert.NotNull(retorno);
            Assert.NotEqual(chave, retorno);
            Assert.Equal(entidadeAcessoCadastrada.RetornarChaveSemCriptografia(), retorno);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.Alterar(entidadeAcessoCadastrada), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);


        }

        [Fact]
        internal async Task DeveRetornarExcecaoAoAlterarChaveQuandoEntidadeAcessoNaoExiste()
        {
            var email = _faker.Person.Email;
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var excecao = await Assert.ThrowsAsync<EntidadeAcessoException>(() => dominio.GerarNovaChaveAsync(email));

            Assert.Equal(NotificacoesEntidadeAcesso.NaoEncontrada.Descricao(), excecao.Message);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.Alterar(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal async Task DeveAtivarOuDesativarEntidadeAcesso(bool ativar)
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(8);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeAcesso = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, !ativar);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.BuscarPorEmailAsync(email)).ReturnsAsync(entidadeAcesso);

            await dominio.AtivarEntidadeAcessoAsync(email, ativar);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            Assert.Equal(ativar, entidadeAcesso.Ativo);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.Alterar(entidadeAcesso), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarExcecaoQuandoTentaAtivarEntidadeAcessoNaoExistente()
        {
            var email = _faker.Person.Email;
            var ativar = _faker.Random.Bool();
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var excecao = await Assert.ThrowsAsync<EntidadeAcessoException>(() => dominio.AtivarEntidadeAcessoAsync(email, ativar));

            Assert.Equal(NotificacoesEntidadeAcesso.NaoEncontrada.Descricao(), excecao.Message);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.Alterar(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }

        [Fact]
        internal async Task DeveRetornarEntidadePorEmail()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(32);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = _faker.Random.Bool();
            var entidadeAcessoCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, dataUltimoAcesso, ativo);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.BuscarPorEmailAsync(email)).ReturnsAsync(entidadeAcessoCadastrada);
            
            var entidadeAcesso = await dominio.BuscarComEmailAsync(email);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            Assert.Equal(entidadeAcesso, entidadeAcessoCadastrada);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExistirEntidadeComEmail()
        {
            var email = _faker.Person.Email;
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var entidadeAcesso = await dominio.BuscarComEmailAsync(email);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarPorEmailAsync(email), Times.Once);
            Assert.Null(entidadeAcesso);
        }
    }
}