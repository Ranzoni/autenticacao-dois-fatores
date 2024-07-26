using System.ComponentModel;

namespace AutenticacaoDoisFatores.Core.Enum
{
    public enum NotificacoesEntidadeAcesso
    {
        [Description("O ID da entidade de acesso é inválido")]
        IdInvalido,
        [Description("O nome da entidade de acesso é inválido, ele deve conter ao menos 3 caracteres e 50 no máximo")]
        NomeInvalido,
        [Description("A chave da entidade de acesso é inválida")]
        ChaveInvalida,
        [Description("O e-mail da entidade de acesso é inválido")]
        EmailInvalido,
        [Description("O e-mail informado já está cadastrado")]
        EmailJaCadastrado,
        [Description("A entidade de acesso não foi encontrada")]
        NaoEncontrada,
        [Description("A entidade de acesso já está ativa")]
        JaAtiva,
        [Description("O e-mail definido como o atual da entidade de acesso não é válido")]
        EmailAtualInvalido,
        [Description("O novo e-mail para a entidade de acesso não é válido")]
        EmailNovoInvalido
    }
}
