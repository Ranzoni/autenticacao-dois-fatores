﻿using AutenticacaoDoisFatores.Core.Entidades;
using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    public class EntidadeAcessoDominio(IEntidadeAcessoRepositorio repositorio) : IEntidadeAcessoDominio
    {
        private readonly IEntidadeAcessoRepositorio _repositorio = repositorio;

        public async Task<EntidadeAcesso> CadastrarAsync(EntidadeAcesso entidadeAcesso)
        {
            var chave = entidadeAcesso.Chave;

            var chaveCriptografada = entidadeAcesso.Chave;
            entidadeAcesso.AlterarChave(chaveCriptografada);

            await _repositorio.CadastrarAsync(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            entidadeAcesso.AlterarChave(chave);
            return entidadeAcesso;
        }

        public async Task<bool> ExisteEntidadeComEmailAsync(string email)
        {
            var existe = await _repositorio.ExisteEntidadeComEmailAsync(email);

            return existe;
        }

        public async Task<EntidadeAcesso?> BuscarComEmailAsync(string email)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);

            return entidadeAcesso;
        }

        public async Task<EntidadeAcesso?> AtivarEntidadeAcessoAsync(string email, bool ativar)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);
            if (entidadeAcesso is null)
                return null;

            entidadeAcesso.Ativar(ativar);

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }

        public async Task<string?> GerarNovaChaveAsync(string email)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);
            if (entidadeAcesso is null)
                return null;

            var novaChave = entidadeAcesso.GerarChave();

            var chaveCriptograda = Criptografia.Criptografar(novaChave);
            entidadeAcesso.AlterarChave(chaveCriptograda);

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return novaChave;
        }

        public async Task<EntidadeAcesso?> AlterarNomeAsync(string email, string nome)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(email);
            if (entidadeAcesso is null)
                return null;

            entidadeAcesso.AlterarNome(nome);

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }

        public async Task<EntidadeAcesso?> AlterarEmailAsync(string emailAtual, string chave, string emailNovo)
        {
            var entidadeAcesso = await _repositorio.BuscarPorEmailAsync(emailAtual);
            if (entidadeAcesso is null)
                return null;

            var chavesSaoIguais = Criptografia.SaoIguais(chave, entidadeAcesso.Chave);
            if (!chavesSaoIguais)
                return null;

            entidadeAcesso.AlterarEmail(emailNovo);

            _repositorio.Alterar(entidadeAcesso);
            await _repositorio.SalvarAlteracoesAsync();

            return entidadeAcesso;
        }
    }
}
