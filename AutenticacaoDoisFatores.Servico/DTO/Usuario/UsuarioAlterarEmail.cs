namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAlterarEmail(string email, Guid chave)
    {
        public string Email { get; private set; } = email;
        public Guid Chave { get; private set; } = chave;
    }
}
