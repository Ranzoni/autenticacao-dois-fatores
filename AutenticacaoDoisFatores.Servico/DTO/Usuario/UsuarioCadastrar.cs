namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioCadastrar(string nome, string email, string senha, Guid chave)
    {
        public string Nome { get; private set; } = nome;
        public string Email { get; private set; } = email;
        public string Senha { get; private set; } = senha;
        public Guid Chave { get; private set; } = chave;
    }
}
