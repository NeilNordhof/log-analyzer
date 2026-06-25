namespace log_analyzer.Types
{
    public class ParsedLine(DateTime timeStamp, LogLevels logLevel, string message)
    {
        public DateTime TimeStamp { get; } = timeStamp;
        public LogLevels LogLevel { get; } = logLevel;
        public string Message { get; } = message;
    }
}
