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

            var email = new MailMessage(de, para, titulo, conteudo);

            var smtpMail = new SmtpClient(host, int.Parse(porta))
            {
                EnableSsl = true
            };
            var cred = new NetworkCredential(de, senha);
            smtpMail.Credentials = cred;

            smtpMail.Send(email);
        }

        internal static void EnviarSucessoCadastroDeAcesso(string para, string chave)
        {
            var titulo = "Cadastro de acesso realizado";

            var conteudo = "O cadastro de acesso foi realizado com sucesso.<br /><br />";
            conteudo += $"Utilize a seguinte chave para realizar as requisições: <b>{chave}</b>";

            Enviar(para, titulo, conteudo);
        }
    }
}
