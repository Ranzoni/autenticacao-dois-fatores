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
            var chaveCriptografia = _faker.Random.AlphaNumeric(16);
            Environment.SetEnvironmentVariable("ENCRYPT_KEY", chaveCriptografia);
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcesso = new EntidadeAcesso(nome, email);
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var retorno = await dominio.CadastrarAsync(entidadeAcesso);

            Assert.NotNull(retorno);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.CadastrarAsync(entidadeAcesso), Times.Once);
            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal async Task DeveRetornarEntidadeSeExistente(bool retornoRep)
        {
            var email = _faker.Person.Email;
            _mock.GetMock<IEntidadeAcessoRepositorio>().Setup(r => r.ExisteEntidadeComEmailAsync(email)).Returns(Task.FromResult(retornoRep));
            var dominio = _mock.CreateInstance<EntidadeAcessoDominio>();

            var retorno = await dominio.ExisteEntidadeComEmailAsync(email);

            _mock.GetMock<IEntidadeAcessoRepositorio>().Verify(r => r.ExisteEntidadeComEmailAsync(email), Times.Once);
            Assert.Equal(retornoRep, retorno);
        }
    }
}
