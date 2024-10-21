using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Autenticacao;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Teste.Construtores;
using Bogus;
using Mensageiro;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Servico
{
    [Collection("Test collection")]
    public class AutenticacaoServicoTeste
    {
        private readonly AutoMocker _mocker = new();
        private readonly Faker _faker = new();

        [Fact]
        internal async Task DeveAutenticar()
        {
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(6);
            var chave = Guid.NewGuid();
            var usuarioAutenticar = new UsuarioAutenticar(email, senha, chave);
            var senhaCriptografada = Criptografia.Criptografar(senha);
            var usuario = new UsuarioConstrutor()
                .ComEmail(email)
                .ComSenha(senhaCriptografada)
                .CriarCompleto();
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarPorEmailAsync(email, chave)).ReturnsAsync(usuario);

            var retorno = await servico.AutenticarAsync(usuarioAutenticar);

            Assert.NotNull(retorno);
            Assert.Equal(email, retorno.Email);
            Assert.NotNull(retorno.Token);
            Assert.NotEmpty(retorno.Token);
            Assert.NotNull(usuario.DataUltimoAcesso);
        }

        [Fact]
        internal async Task NaoDeveAutenticarEmailInexistente()
        {
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(6);
            var chave = Guid.NewGuid();
            var usuarioAutenticar = new UsuarioAutenticar(email, senha, chave);

            var retorno = await servico.AutenticarAsync(usuarioAutenticar);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAutenticarSenhaIncorreta()
        {
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(6);
            var chave = Guid.NewGuid();
            var usuarioAutenticar = new UsuarioAutenticar(email, senha, chave);
            var senhaCriptografada = Criptografia.Criptografar(senha + "abcd");
            var usuario = new UsuarioConstrutor()
                .ComEmail(email)
                .ComSenha(senhaCriptografada)
                .CriarCompleto();
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarPorEmailAsync(email, chave)).ReturnsAsync(usuario);

            var retorno = await servico.AutenticarAsync(usuarioAutenticar);

            Assert.Null(retorno);
            Assert.Null(usuario.DataUltimoAcesso);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoAutorizado(NotificacoesUsuario.SenhaIncorreta), Times.Once);
        }

        [Fact]
        internal async Task DeveAtivar()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var usuario = new UsuarioConstrutor()
                .ComAtivo(false)
                .CriarCompleto();
            var token = Token.GerarTokenConfirmacaoCadastro(usuario.Id, chave);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarNaoAtivoAsync(usuario.Id, chave)).ReturnsAsync(usuario);

            var retorno = await servico.AtivarAsync(token);

            Assert.True(retorno);
            Assert.True(usuario.Ativo);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(usuario), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAtivarQuandoJaAtivo()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var usuario = new UsuarioConstrutor()
                .ComAtivo(true)
                .CriarCompleto();
            var token = Token.GerarTokenConfirmacaoCadastro(usuario.Id, chave);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarNaoAtivoAsync(usuario.Id, chave)).ReturnsAsync(usuario);

            var retorno = await servico.AtivarAsync(token);

            Assert.False(retorno);
            Assert.True(usuario.Ativo);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagem(NotificacoesUsuario.EmailJaCadastrado), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        internal async Task NaoDeveAtivarQuandoUsuarioNaoExiste()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var id = _faker.Random.Int(1);
            var token = Token.GerarTokenConfirmacaoCadastro(id, chave);

            var retorno = await servico.AtivarAsync(token);

            Assert.False(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        internal async Task DeveInativar()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var usuario = new UsuarioConstrutor()
                .ComAtivo(true)
                .CriarCompleto();
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarAtivoAsync(usuario.Id, chave)).ReturnsAsync(usuario);

            var retorno = await servico.InativarAsync(usuario.Id, chave);

            Assert.True(retorno);
            Assert.False(usuario.Ativo);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(usuario), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveInativarQuandoUsuarioNaoExiste()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<AutenticacaoServico>();
            var id = _faker.Random.Int(1);

            var retorno = await servico.InativarAsync(id, chave);

            Assert.False(retorno);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado), Times.Once);
        }
    }
}
