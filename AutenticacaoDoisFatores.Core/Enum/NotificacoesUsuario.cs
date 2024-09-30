﻿using System.ComponentModel;

namespace AutenticacaoDoisFatores.Core.Enum
{
    public enum NotificacoesUsuario
    {
        [Description("O ID do usuário é inválido")]
        IdInvalido,
        [Description("O nome do usuário é inválido, ele deve conter ao menos 3 caracteres e 50 no máximo")]
        NomeInvalido,
        [Description("O e-mail do usuário é inválido")]
        EmailInvalido,
        [Description("Este e-mail já está cadastrado")]
        EmailJaCadastrado,
        [Description("A senha do usuário é inválida")]
        SenhaInvalida,
        [Description("A senha informada para este e-mail está incorreta")]
        SenhaIncorreta,
        [Description("Não foi informada uma chave de acesso relacionada ao usuário")]
        ChaveAcessoNaoInformada,
        [Description("A chave de acesso informada não foi encontrada")]
        ChaveAcessoNaoEncontrada,
        [Description("O usuário não foi encontrado")]
        NaoEncontrado
    }
}
