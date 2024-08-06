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
        }
    }
}
