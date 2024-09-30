namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoExcluir(string email, Guid chave)
    {
        public string Email { get; private set; } = email;
        public Guid Chave { get; private set; } = chave;
    }
}
