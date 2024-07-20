﻿using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoDoisFatores.Infra.Repositorios
{
    public class EntidadeAcessoRepositorio(AutenticacaoDoisFatoresContexto contexto) : RepositorioBase(contexto), IEntidadeAcessoRepositorio
    {
        public void Alterar(EntidadeAcesso entidadeAcesso)
        {
            _contexto.EntidadesAcesso.Update(entidadeAcesso);
        }

        public async Task<EntidadeAcesso?> BuscarPorEmailAsync(string email)
        {
            return await _contexto.EntidadesAcesso.AsNoTracking().FirstOrDefaultAsync(e => e.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task CadastrarAsync(EntidadeAcesso entidadeAcesso)
        {
            await _contexto.AddAsync(entidadeAcesso);
        }

        public async Task<bool> ExisteEntidadeComEmailAsync(string email)
        {
            var existe = await _contexto.EntidadesAcesso.AsNoTracking().AnyAsync(e => e.Email == email);

            return existe;
        }
    }
}
