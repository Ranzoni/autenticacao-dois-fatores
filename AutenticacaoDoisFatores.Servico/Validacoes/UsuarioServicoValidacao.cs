﻿using AutenticacaoDoisFatores.Core.Enum;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Core.Validadores;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Servico.Validacoes
{
    public class UsuarioServicoValidacao(INotificadorServico _notificador, IUsuarioDominio _dominio)
    {
        public async Task<bool> CadastroEhValidoAsync(UsuarioCadastrar usuarioCadastrar)
        {
            if (!UsuarioValidador.NomeEhValido(usuarioCadastrar.Nome))
            {
                _notificador.AddMensagem(NotificacoesUsuario.NomeInvalido);
                return false;
            }

            if (!UsuarioValidador.EmailEhValido(usuarioCadastrar.Email))
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailInvalido);
                return false;
            }

            var emailJaCadastrado = await _dominio.ExisteUsuarioComEmailAsync(usuarioCadastrar.Email);
            if (emailJaCadastrado)
            {
                _notificador.AddMensagem(NotificacoesUsuario.EmailJaCadastrado);
                return false;
            }

            return true;
        }
    }
}
