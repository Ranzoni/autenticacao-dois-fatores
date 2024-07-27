using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;

namespace AutenticacaoDoisFatores.Servico.Servicos
{
    public class NotificadorServico : INotificadorServico
    {
        private readonly List<string> _mensagens = [];

        public void AddMensagem<T>(T mensagem) where T : struct, IConvertible
        {
            var mensagemEmString = mensagem.Descricao() ?? "";
            _mensagens.Add(mensagemEmString);
        }

        public bool ExisteMensagem()
        {
            return _mensagens.Count > 0;
        }

        public IEnumerable<string> Mensagens()
        {
            return _mensagens;
        }
    }
}
