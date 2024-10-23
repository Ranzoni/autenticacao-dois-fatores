namespace AutenticacaoDoisFatores.Servico.DTO.Usuario
{
    public class UsuarioResposta(string nome, string email, DateTime dataCadastro, bool ativo)
    {
        public string Nome { get; private set; } = nome;
        public string Email { get; private set; } = email;
        public DateTime DataCadastro { get; private set; } = dataCadastro;
        public bool Ativo { get; private set; } = ativo;
    }
}
