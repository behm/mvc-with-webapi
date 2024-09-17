using Microsoft.Extensions.Logging;

namespace WebAppWithApi.Logging
{
    public static class DatabaseLoggerExtensions
    {
        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, string connectionString)
        {
            builder.AddProvider(new DatabaseLoggerProvider(connectionString));
            return builder;
        }
    }
}