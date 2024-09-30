using Bogus;

namespace AutenticacaoDoisFatores.Teste
{
    public class TestSetup
    {
        public TestSetup()
        {
            var faker = new Faker();
            var tokenKey = faker.System.ApplePushToken();
            Environment.SetEnvironmentVariable("TOKEN_KEY", tokenKey);
            var chaveAdmin = "###teste_chave_admin$$$";
            Environment.SetEnvironmentVariable("CHAVE_ADMIN", chaveAdmin);
        }
    }
}
