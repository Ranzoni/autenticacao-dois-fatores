namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoCadastrar(string nome, string email)
    {
        public string Nome { get; private set; } = nome;
        public string Email { get; private set; } = email;
    }
}
