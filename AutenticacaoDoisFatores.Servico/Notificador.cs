using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Interfaces;

namespace AutenticacaoDoisFatores.Servico
{
    public class Notificador : INotificador
    {
        private readonly List<string?> _mensagens = [];

        public void AddMensagem<T>(T mensagem) where T : struct, IConvertible
        {
            var mensagemEmString = mensagem.Descricao();
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
