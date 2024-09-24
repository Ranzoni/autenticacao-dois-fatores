namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAlterar(string? nome = null, string? senha = null)
    {
        public string? Nome { get; private set; } = nome;
        public string? Senha { get; private set; } = senha;
    }
}
