namespace AutenticacaoDoisFatores.Servico.Interfaces
{
    public interface INotificador
    {
        void AddMensagem<T>(T mensagem) where T : struct, IConvertible;
        bool ExisteMensagem();
        IEnumerable<string> Mensagens();
    }
}
