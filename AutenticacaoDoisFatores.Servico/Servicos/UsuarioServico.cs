﻿using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.DTO.Usuario;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Utilitarios;
using AutenticacaoDoisFatores.Servico.Validacoes;
using AutoMapper;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class UsuarioServico(IUsuarioDominio _dominio, IEntidadeAcessoDominio _dominioAcesso, IMapper _mapeador, UsuarioServicoValidacao _validacao, IEmailServico _email) : IUsuarioServico
    {
        public async Task<UsuarioResposta?> CadastrarAsync(UsuarioCadastrar usuarioCadastrar, string urlBase)
        {
            var chaveCriptografada = Criptografia.Criptografar(usuarioCadastrar.Chave);
            var entidadeAcesso = await _dominioAcesso.BuscarComChaveAsync(chaveCriptografada);
            if (entidadeAcesso is null)
                return null;

            if (!await _validacao.CadastroEhValidoAsync(usuarioCadastrar))
                return null;

            var senhaCriptografada = Criptografia.Criptografar(usuarioCadastrar.Senha);
            var novoUsuario = new Usuario(usuarioCadastrar.Nome, usuarioCadastrar.Email, senhaCriptografada, entidadeAcesso);

            await _dominio.CadastrarAsync(novoUsuario);

            var token = Token.GerarTokenConfirmacaoCadastro(novoUsuario.Id);
            var linkConfirmacao = $"{urlBase}{token}";
            _email.EnviarEmailConfirmacaoCadastro(novoUsuario.Email, linkConfirmacao);

            var usuarioResposta = _mapeador.Map<UsuarioResposta>(novoUsuario);

            return usuarioResposta;
        }
    }
}