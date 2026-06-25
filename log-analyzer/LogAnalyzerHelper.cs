using log_analyzer.Types;
using System.Text.RegularExpressions;

namespace log_analyzer
{
    public class LogAnalyzerHelper
    {
        private static readonly Regex LogLineRegex = new(@"^(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z) (?<level>[A-Z]+) (?<message>.+)$", RegexOptions.Compiled);
        private static readonly Regex WordSplitRegex = new(@"[^a-zA-Z]+");
        private static readonly HashSet<string> StopWords = ["the", "and", "for", "with", "from", "this", "that", "have", "has", "not", "but", "you", "are", "was", "were", "will", "can", "into", "onto", "over", "under", "between", "a", "an", "of", "to", "in", "on", "at", "by", "is", "it", "as", "be", "or"];

        public static ParsedLine? ParseLine(string line)
        {
            var match = LogLineRegex.Match(line);

            if (!match.Success)
            {
                return null;
            }

            if (Enum.TryParse<LogLevels>(match.Groups["level"].Value, out var logLevel) == false)
            {
                return null;
            }

            return new ParsedLine(DateTime.Parse(match.Groups["timestamp"].Value),
                                  logLevel,
                                  match.Groups["message"].Value);
        }

        public static Dictionary<string, int> CountWords(string message)
        {
            var wordCounts = new Dictionary<string, int>();

            message = message.ToLowerInvariant();
            var words = WordSplitRegex.Split(message);

            foreach (var word in words)
            {
                if (StopWords.Contains(word))
                {
                    continue;
                }

                if (word.Length < 3)
                {
                    continue;
                }

                wordCounts[word] = wordCounts.GetValueOrDefault(word) + 1;
            }
            return wordCounts;
        }
    }
}
