﻿using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class UsuarioDominio(IUsuarioRepositorio _repositorio) : IUsuarioDominio
    {
        public async Task CadastrarAsync(Usuario usuario)
        {
            await _repositorio.CadastrarAsync(usuario);
            await _repositorio.SalvarAlteracoesAsync();
        }
    }
}