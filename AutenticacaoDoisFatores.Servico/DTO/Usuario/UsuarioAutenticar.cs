namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAutenticar(string email, string senha, Guid chave)
    {
        public string Email { get; private set; } = email;
        public string Senha { get; private set; } = senha;
        public Guid Chave { get; private set; } = chave;
    }
}
