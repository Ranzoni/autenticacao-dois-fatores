namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAlterarEmail(string email)
    {
        public string Email { get; private set; } = email;
    }
}
