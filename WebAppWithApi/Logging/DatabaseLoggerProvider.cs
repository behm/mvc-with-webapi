using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace WebAppWithApi.Logging
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();

        public DatabaseLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DatabaseLogger(_connectionString, name));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}