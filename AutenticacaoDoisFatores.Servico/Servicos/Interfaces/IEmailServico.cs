namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IEmailServico
    {
        void EnviarSucessoCadastroDeAcesso(string para, string chave, string linkConfirmacao);
        void EnviarConfirmacaoAlteracaoChaveAcesso(string para, string linkConfirmacao);
        void ReenviarChaveDeAcesso(string para, string chave);
        void EnviarConfirmacaoAlteracaoEntidadeAcesso(string para, string linkConfirmacao);
    }
}
