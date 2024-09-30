using System.Text.RegularExpressions;

namespace AutenticacaoDoisFatores.Core
{
    public static partial class Utilitarios
    {
        public static Regex RetornarEmailRegex()
        {
            return EmailRegex();
        }

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
