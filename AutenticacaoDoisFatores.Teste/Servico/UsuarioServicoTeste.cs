using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Mapeadores;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Teste.Construtores;
using AutoMapper;
using Bogus;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Servico
{
    public class UsuarioServicoTeste
    {
        private readonly AutoMocker _mocker = new();
        private readonly Faker _faker = new();

        public UsuarioServicoTeste()
        {
            var tokenKey = _faker.System.ApplePushToken();
            Environment.SetEnvironmentVariable("TOKEN_KEY", tokenKey);
        }

        [Fact]
        internal async Task DeveCadastrar()
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var entidadeAcesso = EntidadeAcessoConstrutor.CriarEntidadeAcesso();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nome, email, senha, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(It.IsAny<string>())).ReturnsAsync(entidadeAcesso);
            var usuarioResposta = new UsuarioResposta(nome, email, DateTime.Now);
            _mocker.GetMock<IMapper>().Setup(m => m.Map<UsuarioResposta>(It.IsAny<Usuario>())).Returns(usuarioResposta);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.NotNull(retorno);
            Assert.Equal(usuarioResposta, retorno);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Once);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(email, It.IsAny<string>()), Times.Once);
        }
    }
}
