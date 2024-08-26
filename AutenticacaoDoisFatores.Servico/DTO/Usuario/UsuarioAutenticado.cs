namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAutenticado(int id, string email, string token)
    {
        public int Id { get; private set; } = id;
        public string Email { get; private set; } = email;
        public string Token { get; private set; } = token;
    }
}
