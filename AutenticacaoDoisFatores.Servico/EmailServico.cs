using AutenticacaoDoisFatores.Core.Extensoes;
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
                throw new ApplicationException("O host do servidor de e-mail não foi encontrado");

            var porta = Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "";
            if (porta.IsNullOrEmptyOrWhiteSpaces())
                throw new ApplicationException("A porta do servidor de e-mail não foi encontrada");

            var de = Environment.GetEnvironmentVariable("EMAIL_ADDRESS") ?? "";
            if (de.IsNullOrEmptyOrWhiteSpaces())
                throw new ApplicationException("O endereço de e-mail do remetente não foi encontrado para realizar o envio");

            var senha = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? "";
            if (senha.IsNullOrEmptyOrWhiteSpaces())
                throw new ApplicationException("A senha do servidor de e-mail não foi encontrada");

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

        internal static void EnviarSucessoCadastroDeAcesso(string para, string chave)
        {
            var titulo = "Cadastro de acesso realizado";

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
                                        <p>O cadastro de acesso foi realizado com sucesso.</p>
                                        <p>Utilize a seguinte chave para realizar as requisições: <b>{chave}</b></p>
                                    </div>
                                </body>
                             </html>";

            Enviar(para, titulo, conteudo);
        }
    }
}
