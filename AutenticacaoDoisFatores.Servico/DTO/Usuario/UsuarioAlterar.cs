namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioAlterar(string nome, Guid chave)
    {
        public string Nome { get; private set; } = nome;
        public Guid Chave { get; private set; } = chave;
    }
}
