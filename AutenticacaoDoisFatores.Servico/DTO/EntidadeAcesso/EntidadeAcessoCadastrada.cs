namespace AutenticacaoDoisFatores.Servico.DTO.EntidadeAcesso
{
    public class EntidadeAcessoCadastrada(int id, string nome)
    {
        public int Id { get; private set; } = id;
        public string Nome { get; private set; } = nome;
    }
}
