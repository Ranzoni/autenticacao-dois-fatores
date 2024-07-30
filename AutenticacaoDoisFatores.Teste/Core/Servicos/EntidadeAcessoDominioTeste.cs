using AutenticacaoDoisFatores.Core.Entidades;
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

        [Fact]
        internal async Task DeveCriarEntidadeAcesso()
        {
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(32);
            var email = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nome, chave, email);
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
        internal async Task DeveRetornarEntidadePorId()
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
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.BuscarAsync(id)).ReturnsAsync(entidadeAcessoCadastrada);

            var entidadeAcesso = await dominio.BuscarAsync(id);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarAsync(id), Times.Once);
            Assert.Equal(entidadeAcesso, entidadeAcessoCadastrada);
        }

        [Fact]
        internal async Task DeveRetornarNuloQuandoNaoExistirEntidadeComId()
        {
            var id = _faker.Random.Int(1);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var entidadeAcesso = await dominio.BuscarAsync(id);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.BuscarAsync(id), Times.Once);
            Assert.Null(entidadeAcesso);
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

        [Fact]
        internal async Task DeveAlterarEntidade()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(32);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var dataUltimoAcesso = _faker.Date.Recent();
            var ativo = _faker.Random.Bool();
            var entidadeParaAlterar = new EntidadeAcesso(id, nome, chave, email, dataCadastro, dataUltimoAcesso, ativo);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var entidadeAlterada = await dominio.AlterarAsync(entidadeParaAlterar);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.Alterar(entidadeParaAlterar), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        internal async Task DeveExcluirEntidade()
        {
            var id = _faker.Random.Int(1);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.ExcluirAsync(id)).ReturnsAsync(true);

            var excluida = await dominio.ExcluirAsync(id);

            Assert.True(excluida);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.ExcluirAsync(id), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarFalsoQuandoNaoExcluirEntidade()
        {
            var id = _faker.Random.Int(1);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.ExcluirAsync(id)).ReturnsAsync(false);

            var excluida = await dominio.ExcluirAsync(id);

            Assert.False(excluida);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.ExcluirAsync(id), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }
    }
}