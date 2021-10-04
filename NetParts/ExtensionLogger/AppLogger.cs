using System;
using Microsoft.Extensions.Logging;
using NetParts.Models;

namespace NetParts.ExtensionLogger
{
    public class AppLogger : ILogger
    {
        private readonly string _nameCategory;
        private readonly Func<string, LogLevel, bool> _filtro;
        private readonly RepositoryLogger _repository;
        private readonly int _messageMaxLength = 4000;

        public AppLogger(string nameCategory, Func<string, LogLevel, bool> filtro, string connectionString)
        {
            _nameCategory = _nameCategory;
            _filtro = filtro;
            _repository = new RepositoryLogger(connectionString);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventoId,
            TState state, Exception exception, Func<TState, Exception, string> formato)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formato == null)
                throw new ArgumentNullException(nameof(formato));

            var mensagem = formato(state, exception);
            if (string.IsNullOrEmpty(mensagem))
            {
                return;
            }

            if (exception != null)
                mensagem += $"\n{exception.ToString()}";

            mensagem = mensagem.Length > _messageMaxLength ? mensagem.Substring(0, _messageMaxLength) : mensagem;
            var eventLog = new LogEvent()
            {
                Message = mensagem,
                EventId = eventoId.Id,
                LogLevel = logLevel.ToString(),
                CreatedTime = DateTime.UtcNow
            };

            _repository.InsertLog(eventLog);
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filtro == null || _filtro(_nameCategory, logLevel));
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
