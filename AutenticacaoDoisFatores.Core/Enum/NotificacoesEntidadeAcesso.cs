using System.ComponentModel;

namespace AutenticacaoDoisFatores.Core.Enum
{
    public enum NotificacoesEntidadeAcesso
    {
        [Description("O ID da entidade de acesso é inválido")]
        IdInvalido,
        [Description("O nome da entidade de acesso é inválido")]
        NomeInvalido,
        [Description("A chave da entidade de acesso é inválida")]
        ChaveInvalida,
        [Description("O e-mail da entidade de acesso é inválido")]
        EmailInvalido
    }
}
