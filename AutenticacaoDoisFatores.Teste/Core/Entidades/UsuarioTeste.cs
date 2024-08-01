using AutenticacaoDoisFatores.Core.Entidades;
using Bogus;

namespace AutenticacaoDoisFatores.Teste.Core.Entidades
{
    public class UsuarioTeste
    {
        private readonly Faker _faker = new();

        [Fact]
        internal void DeveCriarUsuario()
        {
            var nome = _faker.Person.FullName;
            var email = _faker.Person.Email;
            var senha = _faker.Random.AlphaNumeric(10);
            var entidadeAcesso = CriarEntidadeAcesso();
            var usuario = new Usuario(nome, email, senha, entidadeAcesso);

            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(senha, usuario.Senha);
            Assert.Equal(entidadeAcesso, usuario.EntidadeAcesso);
        }

        private EntidadeAcesso CriarEntidadeAcesso()
        {
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
