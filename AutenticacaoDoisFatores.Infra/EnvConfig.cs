namespace AutenticacaoDoisFatores.Infra
{
    public static class EnvConfig
    {
        public static void Carregar(string caminho)
        {
            if (!File.Exists(caminho))
                return;

            foreach (var linha in File.ReadAllLines(caminho))
            {
                var parte = linha.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parte.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parte[0], parte[1]);
            }
        }
    }
}
