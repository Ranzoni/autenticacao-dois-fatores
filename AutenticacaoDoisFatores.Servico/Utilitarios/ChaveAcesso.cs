namespace AutenticacaoDoisFatores.Servico.Utilitarios
{
    internal static class ChaveAcesso
    {
        private static readonly byte _tamanhoChave = 8;
        private static readonly char[] _caracteres = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

        internal static string GerarChave()
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
    }
}
