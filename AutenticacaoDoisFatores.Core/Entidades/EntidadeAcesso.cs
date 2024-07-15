using AutenticacaoDoisFatores.Core.Servicos;
using System.ComponentModel.DataAnnotations;

namespace AutenticacaoDoisFatores.Core.Entidades
{
    public class EntidadeAcesso: EntidadeBase
    {
        private readonly byte _tamanhoChave = 8;
        private readonly char[] _caracteres = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        private readonly string _chaveSemCriptografia;

        [MaxLength(50)]
        public string Nome { get; private set; }
        public string Chave { get; private set; }

        public EntidadeAcesso(string nome)
        {
            Nome = nome;
            _chaveSemCriptografia = GerarChave();
            Chave = Criptografia.Criptografar(_chaveSemCriptografia);
        }

        public EntidadeAcesso(int id, string nome, string chave)
        {
            Id = id;
            Nome = nome;
            Chave = chave;
        }

        private string GerarChave()
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

            return chave;
        }

        private string RetornarChaveSemCriptografia()
        {
            return _chaveSemCriptografia;
        }
    }
}
