﻿namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoAlterar(string email, string nome)
    {
        public string Email { get; private set; } = email;
        public string Nome { get; private set; } = nome;
    }
}