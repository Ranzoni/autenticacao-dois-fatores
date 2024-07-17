using AutenticacaoDoisFatores.Core.Extensoes;
using System.Net;
using System.Net.Mail;

namespace AutenticacaoDoisFatores.Servico
{
    internal static class EmailServico
    {
        internal static void Enviar(string de, string para, string titulo, string conteudo)
        {
            var email = new MailMessage(de, para, titulo, conteudo);

            var host = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? "";
            var porta = Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "";
            if (host.IsNullOrEmptyOrWhiteSpaces() || porta.IsNullOrEmptyOrWhiteSpaces())
                throw new ApplicationException("As configurações do servidor de e-mail não foram encontradas");

            var smtpMail = new SmtpClient(host, int.Parse(porta))
            {
                EnableSsl = true
            };
            var cred = new NetworkCredential("", "");
            smtpMail.Credentials = cred;

            smtpMail.Send(email);
        }
    }
}
