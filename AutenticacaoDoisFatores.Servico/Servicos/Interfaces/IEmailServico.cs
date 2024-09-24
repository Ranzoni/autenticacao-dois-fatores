namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface IEmailServico
    {
        void EnviarSucessoCadastroDeAcesso(string para, string chave, string linkConfirmacao);
        void EnviarConfirmacaoAlteracaoChaveAcesso(string para, string linkConfirmacao);
        void ReenviarChaveDeAcesso(string para, string chave);
        void EnviarConfirmacaoAlteracaoEntidadeAcesso(string para, string linkConfirmacao);
        void EnviarEmailConfirmacaoCadastro(string para, string linkConfirmacao);
        void EnviarEmailConfirmacaoAlteracaoEmail(string para, string nomeUsuario, string linkConfirmacao);
        void EnviarEmailConfirmacaoAlteracaoSenha(string para, string nomeUsuario, string linkConfirmacao);
    }
}
