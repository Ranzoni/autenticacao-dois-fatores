using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Mapeadores;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutoMapper;
using Bogus;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Servico
{
    public class EntidadeAcessoServicoTeste
    {
        private readonly AutoMocker _mock = new();
        private readonly Faker _faker = new();
        private readonly IMapper _mapeador;

        public EntidadeAcessoServicoTeste()
        {
            var tokenKey = _faker.System.ApplePushToken();
            Environment.SetEnvironmentVariable("TOKEN_KEY", tokenKey);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntidadeAcessoMapeamento>();
            });
            _mapeador = config.CreateMapper();
        }

        [Fact]
        internal async Task DeveCadastrar()
        {
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcessoCadastrar = new EntidadeAcessoCadastrar(nome, email);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var entidadeResposta = new EntidadeAcessoResposta(id, nome, email);
            _mock.GetMock<IMapper>().Setup(m => m.Map<EntidadeAcessoResposta>(It.IsAny<EntidadeAcesso>())).Returns(entidadeResposta);

            var retorno = await servico.CadastrarAsync(entidadeAcessoCadastrar, url);

            Assert.NotNull(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.CadastrarAsync(It.IsAny<EntidadeAcesso>()), Times.Once);
            _mock.GetMock<IEmailServico>().Verify(d => d.EnviarSucessoCadastroDeAcesso(email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal async Task DeveRetornarMensagemQuandoTentarCadastrarNomeInvalido(string nomeInvalido)
        {
            var email = _faker.Person.Email;
            var entidadeAcessoCadastrar = new EntidadeAcessoCadastrar(nomeInvalido, email);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.CadastrarAsync(entidadeAcessoCadastrar, url);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.CadastrarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEmailServico>().Verify(d => d.EnviarSucessoCadastroDeAcesso(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal async Task DeveRetornarMensagemQuandoTentarCadastrarEmailInvalido(string emailInvalido)
        {
            var nome = _faker.Company.CompanyName();
            var entidadeAcessoCadastrar = new EntidadeAcessoCadastrar(nome, emailInvalido);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.CadastrarAsync(entidadeAcessoCadastrar, url);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.CadastrarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEmailServico>().Verify(d => d.EnviarSucessoCadastroDeAcesso(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido), Times.Once);
        }

        [Fact]
        internal async Task DeveRetornarMensagemQuandoTentarCadastrarEmailJaCadastrado()
        {
            var nome = _faker.Company.CompanyName();
            var email = _faker.Person.Email;
            var entidadeAcessoCadastrar = new EntidadeAcessoCadastrar(nome, email);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.ExisteEntidadeComEmailAsync(email)).ReturnsAsync(true);

            var retorno = await servico.CadastrarAsync(entidadeAcessoCadastrar, url);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.CadastrarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEmailServico>().Verify(d => d.EnviarSucessoCadastroDeAcesso(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.EmailJaCadastrado), Times.Once);
        }

        [Fact]
        internal async Task DeveAtivarCadastro()
        {
            var token = _faker.System.ApplePushToken();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.AtivarCadastroAsync(token);

            Assert.NotNull(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.CadastrarAsync(It.IsAny<EntidadeAcesso>()), Times.Once);
            _mock.GetMock<IEmailServico>().Verify(d => d.EnviarSucessoCadastroDeAcesso(email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
