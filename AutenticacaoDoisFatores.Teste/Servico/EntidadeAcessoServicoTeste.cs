using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso;
using AutenticacaoDoisFatores.Servico.Mapeadores;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
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
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, false);
            var entidadeResposta = _mapeador.Map<EntidadeAcessoResposta>(entidadeCadastrada);
            var token = Token.GerarTokenEnvioChaveAcesso(email);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.AlterarAsync(entidadeCadastrada)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IMapper>().Setup(m => m.Map<EntidadeAcessoResposta>(entidadeCadastrada)).Returns(entidadeResposta);

            var retorno = await servico.AtivarCadastroAsync(token);

            Assert.NotNull(retorno);
            Assert.True(entidadeCadastrada.Ativo);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(entidadeCadastrada), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAtivarCadastroQuandoNaoEncontrarEntidade()
        {
            var email = _faker.Person.Email;
            var token = Token.GerarTokenEnvioChaveAcesso(email);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.AtivarCadastroAsync(token);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
        }

        [Fact]
        internal async Task NaoDeveAtivarCadastroQuandoEntidadeJaAtiva()
        {
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, true);
            var token = Token.GerarTokenEnvioChaveAcesso(email);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.AtivarCadastroAsync(token);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.JaAtiva), Times.Once);
        }

        [Fact]
        internal async Task DeveReenviarChaveAcesso()
        {
            var email = _faker.Person.Email;
            var reenviarChaveAcesso = new ReenviarChaveAcesso(email);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.ReenviarChaveAcessoAsync(reenviarChaveAcesso, url);

            Assert.True(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoChaveAcesso(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveReenviarChaveAcessoQuandoEntidadeNaoExiste()
        {
            var email = _faker.Person.Email;
            var reenviarChaveAcesso = new ReenviarChaveAcesso(email);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.ReenviarChaveAcessoAsync(reenviarChaveAcesso, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoChaveAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task DeveAlterarChaveAcesso()
        {
            var id = _faker.Random.Int(1);
            var token = Token.GerarTokenReenvioChave(id);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarAsync(id)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.AlterarChaveAcessoAsync(token);

            Assert.True(retorno);
            Assert.NotEqual(chave, entidadeCadastrada.Chave);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(entidadeCadastrada), Times.Once);
            _mock.GetMock<IEmailServico>().Verify(e => e.ReenviarChaveDeAcesso(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAlterarChaveAcessoQuandoEntidadeNaoExiste()
        {
            var id = _faker.Random.Int(1);
            var token = Token.GerarTokenReenvioChave(id);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.AlterarChaveAcessoAsync(token);

            Assert.False(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
            _mock.GetMock<IEmailServico>().Verify(e => e.ReenviarChaveDeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task DeveEnviarEmailAlteracaoNome()
        {
            var email = _faker.Person.Email;
            var novoNome = _faker.Company.CompanyName();
            var entidadeAlterarNome = new EntidadeAcessoAlterarNome(email, novoNome);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nomeAtual = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nomeAtual, chave, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.EnviarEmailAlteracaoNomeAsync(entidadeAlterarNome, url);

            Assert.True(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveEnviarEmailAlteracaoNomeQuandoEntidadeNaoExiste()
        {
            var email = _faker.Person.Email;
            var nome = _faker.Company.CompanyName();
            var entidadeAlterarNome = new EntidadeAcessoAlterarNome(email, nome);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.EnviarEmailAlteracaoNomeAsync(entidadeAlterarNome, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal async Task NaoDeveEnviarEmailAlteracaoNomeQuandoNomeInvalido(string nomeInvalido)
        {
            var email = _faker.Person.Email;
            var entidadeAlterarNome = new EntidadeAcessoAlterarNome(email, nomeInvalido);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nomeAtual = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nomeAtual, chave, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.EnviarEmailAlteracaoNomeAsync(entidadeAlterarNome, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.NomeInvalido), Times.Once);
        }

        [Fact]
        internal async Task DeveAlterarNome()
        {
            var id = _faker.Random.Int(1);
            var novoNome = _faker.Company.CompanyName();
            var token = Token.GerarTokenAlterarNomeEntidadeAcesso(id, novoNome);
            var email = _faker.Person.Email;
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var nomeAtual = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nomeAtual, chave, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarAsync(id)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.AlterarAsync(entidadeCadastrada)).ReturnsAsync(entidadeCadastrada);
            var entidadeResposta = new EntidadeAcessoResposta(id, novoNome, email);
            _mock.GetMock<IMapper>().Setup(m => m.Map<EntidadeAcessoResposta?>(entidadeCadastrada)).Returns(entidadeResposta);

            var retorno = await servico.AlterarNomeAsync(token);

            Assert.NotNull(retorno);
            Assert.Equal(entidadeResposta, retorno);
            Assert.Equal(novoNome, entidadeCadastrada.Nome);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(entidadeCadastrada), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAlterarNomeQuandoEntidadeNaoExiste()
        {
            var id = _faker.Random.Int(1);
            var novoNome = _faker.Company.CompanyName();
            var token = Token.GerarTokenAlterarNomeEntidadeAcesso(id, novoNome);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.AlterarNomeAsync(token);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
        }

        [Fact]
        internal async Task DeveEnviarEmailAlteracaoEmail()
        {
            var emailAtual = _faker.Person.Email;
            var emailNovo = $"novo_{_faker.Person.Email}";
            var chave = _faker.Random.AlphaNumeric(32);
            var entidadeAlterarEmail = new EntidadeAcessoAlterarEmail(emailAtual, emailNovo, chave);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var nomeAtual = _faker.Company.CompanyName();
            var chaveCriptografada = Criptografia.Criptografar(chave);
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nomeAtual, chaveCriptografada, emailAtual, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(emailAtual)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.EnviarEmailAlteracaoEmailAsync(entidadeAlterarEmail, url);

            Assert.True(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(emailNovo, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveEnviarEmailAlteracaoEmailQuandoEntidadeNaoExiste()
        {
            var emailAtual = _faker.Person.Email;
            var emailNovo = $"novo_{_faker.Person.Email}";
            var chave = _faker.Random.AlphaNumeric(32);
            var entidadeAlterarEmail = new EntidadeAcessoAlterarEmail(emailAtual, emailNovo, chave);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.EnviarEmailAlteracaoEmailAsync(entidadeAlterarEmail, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal async Task NaoDeveEnviarEmailAlteracaoEmailQuandoEhInvalido(string emailInvalido)
        {
            var emailAtual = _faker.Person.Email;
            var chave = _faker.Random.AlphaNumeric(32);
            var entidadeAlterarEmail = new EntidadeAcessoAlterarEmail(emailAtual, emailInvalido, chave);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, emailAtual, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(emailAtual)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.EnviarEmailAlteracaoEmailAsync(entidadeAlterarEmail, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mock.GetMock<INotificadorServico>().Verify(n => n.AddMensagem(NotificacoesEntidadeAcesso.EmailInvalido), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveEnviarEmailAlteracaoEmailQuandoChaveNaoValida()
        {
            var emailAtual = _faker.Person.Email;
            var emailNovo = $"novo_{_faker.Person.Email}";
            var chave = _faker.Random.AlphaNumeric(32);
            var entidadeAlterarEmail = new EntidadeAcessoAlterarEmail(emailAtual, emailNovo, chave);
            var url = _faker.Internet.Url();
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var nomeAtual = _faker.Company.CompanyName();
            var chaveCriptografada = Criptografia.Criptografar($"{_faker.Random.AlphaNumeric(28)}novo");
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nomeAtual, chaveCriptografada, emailAtual, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(emailAtual)).ReturnsAsync(entidadeCadastrada);

            var retorno = await servico.EnviarEmailAlteracaoEmailAsync(entidadeAlterarEmail, url);

            Assert.False(retorno);
            _mock.GetMock<IEmailServico>().Verify(e => e.EnviarConfirmacaoAlteracaoEntidadeAcesso(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task DeveAlterarEmail()
        {
            var id = _faker.Random.Int(1);
            var novoEmail = $"novo_{_faker.Person.Email}";
            var token = Token.GerarTokenAlterarEmailEntidadeAcesso(id, novoEmail);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var emailAtual = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chave, emailAtual, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarAsync(id)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.AlterarAsync(entidadeCadastrada)).ReturnsAsync(entidadeCadastrada);
            var entidadeResposta = new EntidadeAcessoResposta(id, nome, novoEmail);
            _mock.GetMock<IMapper>().Setup(m => m.Map<EntidadeAcessoResposta?>(entidadeCadastrada)).Returns(entidadeResposta);

            var retorno = await servico.AlterarEmailAsync(token);

            Assert.NotNull(retorno);
            Assert.Equal(entidadeResposta, retorno);
            Assert.Equal(novoEmail, entidadeCadastrada.Email);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(entidadeCadastrada), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAlterarEmailQuandoEntidadeNaoExiste()
        {
            var id = _faker.Random.Int(1);
            var novoEmail = $"novo_{_faker.Person.Email}";
            var token = Token.GerarTokenAlterarEmailEntidadeAcesso(id, novoEmail);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();

            var retorno = await servico.AlterarEmailAsync(token);

            Assert.Null(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.AlterarAsync(It.IsAny<EntidadeAcesso>()), Times.Never);
        }

        [Fact]
        internal async Task DeveExcluir()
        {
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var chaveCriptografada = Criptografia.Criptografar(chave);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chaveCriptografada, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.ExcluirAsync(id)).ReturnsAsync(true);
            var entidadeAcessoExcluir = new EntidadeAcessoExcluir(email, chave);

            var retorno = await servico.ExcluirAsync(entidadeAcessoExcluir);

            Assert.True(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.ExcluirAsync(id), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveExcluirQuandoEntidadeNaoExiste()
        {
            var id = _faker.Random.Int(1);
            var email = _faker.Person.Email;
            var chave = _faker.Random.AlphaNumeric(16);
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var entidadeAcessoExcluir = new EntidadeAcessoExcluir(email, chave);

            var retorno = await servico.ExcluirAsync(entidadeAcessoExcluir);

            Assert.False(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.ExcluirAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        internal async Task NaoDeveExcluirQuandoChaveNaoValida()
        {
            var servico = _mock.CreateInstance<EntidadeAcessoServico>();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(16);
            var chaveCriptografada = Criptografia.Criptografar(chave);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();
            var entidadeCadastrada = new EntidadeAcesso(id, nome, chaveCriptografada, email, dataCadastro, null, true);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComEmailAsync(email)).ReturnsAsync(entidadeCadastrada);
            _mock.GetMock<IEntidadeAcessoDominio>().Setup(d => d.ExcluirAsync(id)).ReturnsAsync(true);
            chave += "diferente";
            var entidadeAcessoExcluir = new EntidadeAcessoExcluir(email, chave);

            var retorno = await servico.ExcluirAsync(entidadeAcessoExcluir);

            Assert.False(retorno);
            _mock.GetMock<IEntidadeAcessoDominio>().Verify(d => d.ExcluirAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
