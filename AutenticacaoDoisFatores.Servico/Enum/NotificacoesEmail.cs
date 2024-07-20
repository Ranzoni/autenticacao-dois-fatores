using System.ComponentModel;

namespace AutenticacaoDoisFatores.Servico.Enum
{
    public enum NotificacoesEmail
    {
        [Description("O host do servidor de e-mail não foi encontrado")]
        HostNaoEncontrado,
        [Description("A porta do servidor de e-mail não foi encontrada")]
        PortaNaoEncontrada,
        [Description("O endereço de e-mail do remetente não foi encontrado para realizar o envio")]
        EmailRemetenteNaoEncontrado,
        [Description("A senha do servidor de e-mail não foi encontrada")]
        SenhaEmailNaoEncontrada
    }
}
