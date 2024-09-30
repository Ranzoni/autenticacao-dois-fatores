using System.ComponentModel;

namespace AutenticacaoDoisFatores.Core.Enum
{
    public enum NotificacoesEntidadeAcesso
    {
        [Description("O ID da entidade de acesso é inválido")]
        IdInvalido,
        [Description("O nome da entidade de acesso é inválido, ele deve conter ao menos 3 caracteres e 50 no máximo")]
        NomeInvalido,
        [Description("O e-mail da entidade de acesso é inválido")]
        EmailInvalido,
        [Description("O e-mail informado já está cadastrado")]
        EmailJaCadastrado,
        [Description("A entidade de acesso já está ativa")]
        JaAtiva,
        [Description("A entidade não foi encontrada")]
        NaoEncontrada
    }
}
