﻿using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Teste.Construtores;
using AutoMapper;
using Bogus;
using Mensageiro;
using Moq;
using Moq.AutoMock;

namespace AutenticacaoDoisFatores.Teste.Servico
{
    [Collection("Test collection")]
    public class UsuarioServicoTeste
    {
        private readonly AutoMocker _mocker = new();
        private readonly Faker _faker = new();

        [Fact]
        internal async Task DeveCadastrar()
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var construtor = new EntidadeAcessoConstrutor();
            var chave = Guid.NewGuid();
            var entidadeAcesso = construtor
                .ComChave(chave)
                .CriarEntidadeAcesso();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nome, email, senha, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(chave)).ReturnsAsync(entidadeAcesso);
            var usuarioResposta = new UsuarioResposta(nome, email, DateTime.Now);
            _mocker.GetMock<IMapper>().Setup(m => m.Map<UsuarioResposta>(It.IsAny<Usuario>())).Returns(usuarioResposta);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.NotNull(retorno);
            Assert.Equal(usuarioResposta, retorno);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Once);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(email, It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("123456789012345678901234567890123456789012345678901")]
        internal async Task NaoDeveCadastrarComNomeInvalido(string nomeInvalido)
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor.CriarEntidadeAcesso();
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nomeInvalido, email, senha, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(entidadeAcesso.Chave)).ReturnsAsync(entidadeAcesso);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagem(NotificacoesUsuario.NomeInvalido), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("a@.c")]
        [InlineData("a@12345678901234567890123456789012345678901234567890123456789012345678901234567.c")]
        internal async Task NaoDeveCadastrarComEmailInvalido(string emailInvalido)
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor.CriarEntidadeAcesso();
            var nome = _faker.Person.FullName;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nome, emailInvalido, senha, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(entidadeAcesso.Chave)).ReturnsAsync(entidadeAcesso);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagem(NotificacoesUsuario.EmailInvalido), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        internal async Task NaoDeveCadastrarComSenhaInvalida(string senhaInvalida)
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor.CriarEntidadeAcesso();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var usuarioCadastrar = new UsuarioCadastrar(nome, email, senhaInvalida, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(entidadeAcesso.Chave)).ReturnsAsync(entidadeAcesso);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagem(NotificacoesUsuario.SenhaInvalida), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task NaoDeveCadastrarQuandoEmailJaEstaCadastrado()
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var construtor = new EntidadeAcessoConstrutor();
            var entidadeAcesso = construtor.CriarEntidadeAcesso();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nome, email, senha, entidadeAcesso.Chave);
            _mocker.GetMock<IEntidadeAcessoDominio>().Setup(d => d.BuscarComChaveAsync(entidadeAcesso.Chave)).ReturnsAsync(entidadeAcesso);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.ExisteUsuarioComEmailAsync(email, entidadeAcesso.Chave)).ReturnsAsync(true);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagem(NotificacoesUsuario.EmailJaCadastrado), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task NaoDeveCadastrarComChaveInvalida()
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var chave = Guid.NewGuid();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrar = new UsuarioCadastrar(nome, email, senha, chave);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.ExisteUsuarioComEmailAsync(email, chave)).ReturnsAsync(true);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.CadastrarAsync(usuarioCadastrar, urlBase);

            Assert.Null(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoEncontrado(NotificacoesUsuario.ChaveAcessoNaoEncontrada), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.CadastrarAsync(It.IsAny<Usuario>()), Times.Never);
            _mocker.GetMock<IEmailServico>().Verify(s => s.EnviarEmailConfirmacaoCadastro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        internal async Task DeveAtivar()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var usuario = new UsuarioConstrutor()
                .ComAtivo(false)
                .CriarCompleto();
            var token = Token.GerarTokenConfirmacaoCadastro(usuario.Id, chave);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarAsync(usuario.Id, chave)).ReturnsAsync(usuario);

            var retorno = await servico.AtivarAsync(token);

            Assert.True(retorno);
            Assert.True(usuario.Ativo);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(usuario), Times.Once);
        }

        [Fact]
        internal async Task NaoDeveAtivarQuandoJaAtivo()
        {
            var chave = Guid.NewGuid();
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var usuario = new UsuarioConstrutor()
                .ComAtivo(true)
                .CriarCompleto();
            var token = Token.GerarTokenConfirmacaoCadastro(usuario.Id, chave);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarAsync(usuario.Id, chave)).ReturnsAsync(usuario);

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
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var id = _faker.Random.Int(1);
            var token = Token.GerarTokenConfirmacaoCadastro(id, chave);

            var retorno = await servico.AtivarAsync(token);

            Assert.False(retorno);
            _mocker.GetMock<INotificador>().Verify(n => n.AddMensagemNaoEncontrado(NotificacoesUsuario.NaoEncontrado), Times.Once);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        internal async Task DeveAlterar()
        {
            var servico = _mocker.CreateInstance<UsuarioServico>();
            var id = _faker.Random.Int(1);
            var chave = Guid.NewGuid();
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var usuarioCadastrado = new UsuarioConstrutor()
                .ComId(id)
                .ComNome(nome)
                .ComEmail(email)
                .ComSenha(senha)
                .CriarCompleto();
            var novoNome = $"{_faker.Person.FullName} Silva";
            var usuarioAlterar = new UsuarioAlterar(novoNome, chave);
            var usuarioResposta = new UsuarioResposta(nome, email, DateTime.Now);
            _mocker.GetMock<IUsuarioDominio>().Setup(d => d.BuscarAsync(id, chave)).ReturnsAsync(usuarioCadastrado);
            _mocker.GetMock<IMapper>().Setup(m => m.Map<UsuarioResposta>(It.IsAny<Usuario>())).Returns(usuarioResposta);
            var urlBase = _faker.Internet.Url();

            var retorno = await servico.AlterarAsync(id, usuarioAlterar);

            Assert.NotNull(retorno);
            Assert.Equal(usuarioResposta, retorno);
            Assert.Equal(novoNome, usuarioCadastrado.Nome);
            _mocker.GetMock<IUsuarioDominio>().Verify(d => d.AlterarAsync(It.IsAny<Usuario>()), Times.Once);
        }
    }
}
