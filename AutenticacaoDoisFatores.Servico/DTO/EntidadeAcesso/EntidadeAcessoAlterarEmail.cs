namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoAlterarEmail(string emailAtual, string emailNovo, string chave)
    {
        public string EmailAtual { get; private set; } = emailAtual;
        public string EmailNovo { get; private set; } = emailNovo;
        public string Chave { get; private set; } = chave;
    }
}
