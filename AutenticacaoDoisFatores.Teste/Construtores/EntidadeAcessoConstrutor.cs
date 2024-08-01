using AutenticacaoDoisFatores.Core.Entidades;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Construtores
{
    internal static class EntidadeAcessoConstrutor
    {
        internal static EntidadeAcesso CriarEntidadeAcesso()
        {
            var _faker = new Faker();
            var id = _faker.Random.Int(1);
            var nome = _faker.Company.CompanyName();
            var chave = _faker.Random.AlphaNumeric(32);
            var email = _faker.Person.Email;
            var dataCadastro = _faker.Date.Past();

            var entidadeAcesso = new EntidadeAcesso(id, nome, chave, email, dataCadastro, null, true);

            return entidadeAcesso;
        }
    }
}
