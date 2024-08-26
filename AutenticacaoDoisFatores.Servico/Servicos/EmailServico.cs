﻿using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Excecoes;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using System.Net;
using System.Net.Mail;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class EmailServico : IEmailServico
    {
        private readonly string _host;
        private readonly string _porta;
        private readonly string _emailRemetente;
        private readonly string _senha;

        public EmailServico()
        {
            var host = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? "";
            if (host.IsNullOrEmptyOrWhiteSpaces())
                EmailServicoException.HostNaoEncontrado();

            var porta = Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "";
            if (porta.IsNullOrEmptyOrWhiteSpaces())
                EmailServicoException.PortaNaoEncontrada();

            var de = Environment.GetEnvironmentVariable("EMAIL_ADDRESS") ?? "";
            if (de.IsNullOrEmptyOrWhiteSpaces())
                EmailServicoException.EmailRemetenteNaoEncontrado();

            var senha = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? "";
            if (senha.IsNullOrEmptyOrWhiteSpaces())
                EmailServicoException.SenhaEmailNaoEncontrada();

            _host = host;
            _porta = porta;
            _emailRemetente = de;
            _senha = senha;
        }

        private void Enviar(string para, string titulo, string conteudo)
        {
            var email = new MailMessage(_emailRemetente, para, titulo, conteudo)
            {
                IsBodyHtml = true
            };

            var smtpMail = new SmtpClient(_host, int.Parse(_porta))
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailRemetente, _senha)
            };

            smtpMail.Send(email);
        }

        public void EnviarSucessoCadastroDeAcesso(string para, string chave, string linkConfirmacao)
        {
            var titulo = "Cadastro de acesso realizado";
            var mensagem = $@"<p>O cadastro de acesso foi realizado com sucesso.</p>
                              <p>Utilize a seguinte chave para realizar as requisições: <b>{chave}</b></p>
                              <p>Para confirmar a sua chave de acesso clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>";
            var conteudo = HtmlMensagem(mensagem);

            Enviar(para, titulo, conteudo);
        }

        public void EnviarConfirmacaoAlteracaoChaveAcesso(string para, string linkConfirmacao)
        {
            var titulo = "Confirmar nova chave de acesso";
            var conteudo = HtmlMensagem($"<p>Para confirmar a geração da nova chave de acesso clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>");

            Enviar(para, titulo, conteudo);
        }

        public void ReenviarChaveDeAcesso(string para, string chave)
        {
            var titulo = "Reenvio de chave de acesso";
            var conteudo = HtmlMensagem($"<p>Utilize a seguinte chave para realizar as requisições: <b>{chave}</b></p>");

            Enviar(para, titulo, conteudo);
        }

        public void EnviarConfirmacaoAlteracaoEntidadeAcesso(string para, string linkConfirmacao)
        {
            var titulo = "Confirmar alteração nos dados de acesso";
            var conteudo = HtmlMensagem($"<p>Para confirmar a geração da nova chave de acesso clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>");

            Enviar(para, titulo, conteudo);
        }

        private static string HtmlMensagem(string mensagem)
        {
            var styleCss = @"<style>
                                body {font-family: Arial, sans-serif; }
                                h1 {color: #333; }
                                p {color: #555; }
                                .content {padding: 10px; border: 1px solid #ccc; }
                            </style>";

            var conteudo = $@"<html>
                                <head>
                                    {styleCss}
                                </head>
                                <body>
                                    <div class='content'>
                                        {mensagem}
                                    </div>
                                </body>
                             </html>";

            return conteudo;
        }

        public void EnviarEmailConfirmacaoCadastro(string para, string linkConfirmacao)
        {
            var titulo = "Confirmação de Cadastro";
            var conteudo = HtmlMensagem($"<p>Para confirmar o cadastro do seu usuário clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>");

            Enviar(para, titulo, conteudo);
        }

        public void EnviarEmailConfirmacaoAlteracaoEmail(string para, string nomeUsuario, string linkConfirmacao)
        {
            var titulo = "Alteração de E-mail do Usuário";
            var conteudo = HtmlMensagem($"<p>Olá, {nomeUsuario}.<br/ ><br />Para confirmar o novo e-mail do seu usuário clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>");

            Enviar(para, titulo, conteudo);
        }
    }
}
