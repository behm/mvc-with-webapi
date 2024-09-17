using System;

namespace WebAppWithApi.Logging
{
    public class LogEntry
    {
        public string LogLevel { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public int EventId { get; set; }
        public string Exception { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}