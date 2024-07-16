namespace AutenticacaoDoisFatores.Core.Excecoes
{
    internal class EntidadeAcessoException : ApplicationException
    {
        public EntidadeAcessoException(string? message) : base(message)
        {
        }

        internal static void IdInvalido()
        {
            throw new EntidadeAcessoException("O ID da entidade de acesso é inválido");
        }

        internal static void NomeInvalido()
        {
            throw new EntidadeAcessoException("O nome da entidade de acesso é inválido");
        }

        internal static void ChaveInvalida()
        {
            throw new EntidadeAcessoException("A chave da entidade de acesso é inválida");
        }

        internal static void EmailInvalido()
        {
            throw new EntidadeAcessoException("O e-mail da entidade de acesso é inválido");
        }
    }
}
