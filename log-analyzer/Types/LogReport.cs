namespace log_analyzer.Types
{
    public class LogReport
    {
        public int TotalLines { get; set; } = 0;
        public Dictionary<LogLevels, int> LogLevelCounts { get; } = Enum.GetValues<LogLevels>().ToDictionary(level => level, _ => 0);
        public int MalformedCount { get; set; } = 0;
        public DateTime LastErrorDateTime { get; set; } = DateTime.MinValue;
        public string LastErrorMessage { get; set; } = string.Empty;
        public Dictionary<string, int> WordOccurences { get; } = [];

    }
}
