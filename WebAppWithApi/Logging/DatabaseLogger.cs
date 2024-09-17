using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace WebAppWithApi.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _categoryName;

        public DatabaseLogger(string connectionString, string categoryName)
        {
            _connectionString = connectionString;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var logEntry = new LogEntry
            {
                LogLevel = logLevel.ToString(),
                Category = _categoryName,
                Message = message,
                EventId = eventId.Id,
                Exception = exception?.ToString(),
                CreatedTime = DateTime.UtcNow,
            };

            WriteLogToDatabase(logEntry);
        }

        private void WriteLogToDatabase(LogEntry logEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Logs (LogLevel, Category, Message, EventId, Exception, CreatedTime) VALUES (@LogLevel, @Category, @Message, @EventId, @Exception, @CreatedTime)", connection);
                command.Parameters.AddWithValue("@LogLevel", logEntry.LogLevel);
                command.Parameters.AddWithValue("@Category", logEntry.Category);
                command.Parameters.AddWithValue("@Message", logEntry.Message);
                command.Parameters.AddWithValue("@EventId", logEntry.EventId);
                command.Parameters.AddWithValue("@Exception", logEntry.Exception);
                command.Parameters.AddWithValue("@CreatedTime", logEntry.CreatedTime);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}