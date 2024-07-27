namespace AutenticacaoDoisFatores.Servico.Servicos.Interfaces
{
    public interface INotificadorServico
    {
        void AddMensagem<T>(T mensagem) where T : struct, IConvertible;
        bool ExisteMensagem();
        IEnumerable<string> Mensagens();
    }
}
