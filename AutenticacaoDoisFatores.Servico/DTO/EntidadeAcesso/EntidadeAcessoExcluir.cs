namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoExcluir(string email, string chave)
    {
        public string Email { get; private set; } = email;
        public string Chave { get; private set; } = chave;
    }
}
