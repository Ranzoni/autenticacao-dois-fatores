using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Excecoes;
using System;
using System.Net;
using System.Net.Mail;

namespace AutenticacaoDoisFatores.Servico
{
    internal static class EmailServico
    {
        private static void Enviar(string para, string titulo, string conteudo)
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

            var email = new MailMessage(de, para, titulo, conteudo)
            {
                IsBodyHtml = true
            };

            var smtpMail = new SmtpClient(host, int.Parse(porta))
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(de, senha)
            };

            smtpMail.Send(email);
        }

        internal static void EnviarSucessoCadastroDeAcesso(string para, string chave, string linkConfirmacao)
        {
            var titulo = "Cadastro de acesso realizado";
            var mensagem = $@"<p>O cadastro de acesso foi realizado com sucesso.</p>
                              <p>Utilize a seguinte chave para realizar as requisições: <b>{chave}</b></p>
                              <p>Para confirmar a sua chave de acesso clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>";
            var conteudo = HtmlMensagem(mensagem);

            Enviar(para, titulo, conteudo);
        }

        internal static void EnviarConfirmacaoAlteracaoChaveAcesso(string para, string linkConfirmacao)
        {
            var titulo = "Confirmar nova chave de acesso";
            var conteudo = HtmlMensagem($"<p>Para confirmar a geração da nova chave de acesso clique no seguinte link: <a href='{linkConfirmacao}'>Clique aqui!</a></p>");

            Enviar(para, titulo, conteudo);
        }

        internal static void ReenviarChaveDeAcesso(string para, string chave)
        {
            var titulo = "Reenvio de chave de acesso";
            var conteudo = HtmlMensagem($"<p>Utilize a seguinte chave para realizar as requisições: <b>{chave}</b></p>");

            Enviar(para, titulo, conteudo);
        }

        internal static void EnviarConfirmacaoAlteracaoEntidadeAcesso(string para, string linkConfirmacao)
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
    }
}
