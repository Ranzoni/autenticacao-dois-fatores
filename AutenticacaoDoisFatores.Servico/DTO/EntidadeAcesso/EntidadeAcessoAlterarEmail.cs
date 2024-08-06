namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoAlterarEmail(string emailAtual, string emailNovo, Guid chave)
    {
        public string EmailAtual { get; private set; } = emailAtual;
        public string EmailNovo { get; private set; } = emailNovo;
        public Guid Chave { get; private set; } = chave;
    }
}
