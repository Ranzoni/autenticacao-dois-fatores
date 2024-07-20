using AutenticacaoDoisFatores.Core.Servicos;
using AutenticacaoDoisFatores.Core.Validadores;
using System.ComponentModel.DataAnnotations;

namespace AutenticacaoDoisFatores.Core.Entidades
{
    public class EntidadeAcesso: EntidadeBase
    {
        private readonly byte _tamanhoChave = 8;
        private readonly char[] _caracteres = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        private string? _chaveSemCriptografia;

        public string Nome { get; private set; }
        public string Chave { get; private set; }
        public string Email { get; private set; }

        public EntidadeAcesso(string nome, string email)
        {
            EntidadeAcessoValidador.ValidarNovaEntidade(nome, email);

            Chave = "";
            Nome = nome;
            Email = email;
            GerarChave();
        }

        public EntidadeAcesso(int id, string nome, string chave, string email)
        {
            EntidadeAcessoValidador.ValidarEntidadeCompleta(id, nome, chave, email);

            Id = id;
            Nome = nome;
            Chave = chave;
            Email = email;
        }

        internal void GerarChave()
        {
            var chave = "";
            var geradorRandomicoNum = new Random();
            var qtdTotalCaracteres = _caracteres.Length - 1;

            for (var i = 1; i <= _tamanhoChave; i++)
            {
                var idxRandomico = geradorRandomicoNum.Next(qtdTotalCaracteres);
                var carectereSorteado = _caracteres[idxRandomico];

                chave += carectereSorteado;
            }

            _chaveSemCriptografia = chave;
            Chave = Criptografia.Criptografar(_chaveSemCriptografia);
        }

        public string RetornarChaveSemCriptografia()
        {
            return _chaveSemCriptografia ?? "";
        }
    }
}
