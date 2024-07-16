namespace AutenticacaoDoisFatores.Core.Extensoes
{
    public static class ExtensaoString
    {
        public static bool IsNullOrEmptyOrWhiteSpaces(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}
