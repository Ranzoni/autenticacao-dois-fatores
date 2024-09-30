namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAlterarSenha(string senha, Guid chave)
    {
        public string Senha { get; private set; } = senha;
        public Guid Chave { get; private set; } = chave;
    }
}
