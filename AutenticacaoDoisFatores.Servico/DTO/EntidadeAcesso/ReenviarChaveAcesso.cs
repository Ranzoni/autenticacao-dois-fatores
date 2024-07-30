namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class ReenviarChaveAcesso(string email)
    {
        public string Email { get; private set; } = email;
    }
}
