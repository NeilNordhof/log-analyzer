using log_analyzer;

namespace log_analyzer_tests
{
    public class LogAnalyzerTests
    {
        [Theory]
        [InlineData("2024-06-01T12:34:56Z INFO This is a test log message.")]
        [InlineData("2024-06-01T12:34:56Z ERROR An error occurred.")]
        [InlineData("2024-06-01T12:34:56Z WARN This is a warning.")]
        public void Parse_ValidLine_ReturnsParsedLine(string logLine)
        {
            var result = LogAnalyzerHelper.ParseLine(logLine);
            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_InvalidLine_ReturnsNull()
        {
            var result = LogAnalyzerHelper.ParseLine("Invalid log line");
            Assert.Null(result);
        }

        [Fact]
        public void Parse_UnknownLogLevel_ReturnsNull()
        {
            var result = LogAnalyzerHelper.ParseLine("2024-06-01T12:34:56Z UNKNOWN This is a test log message.");
            Assert.Null(result);
        }

        [Fact]
        public void CountWords_ValidMessage_ReturnsWordCounts()
        {
            var message = "This is a test log message. This message is for testing.";
            var result = LogAnalyzerHelper.CountWords(message);
            Assert.Equal(2, result["message"]);
            Assert.Equal(1, result["test"]);
            Assert.Equal(1, result["log"]);
            Assert.Equal(1, result["testing"]);
        }

        [Fact]
        public void CountWords_MessageWithStopWords_ReturnsWordCountsExcludingStopWords()
        {
            var message = "This is a test log message with the and for.";
            var result = LogAnalyzerHelper.CountWords(message);
            Assert.Equal(1, result["test"]);
            Assert.Equal(1, result["log"]);
            Assert.Equal(1, result["message"]);
            Assert.False(result.ContainsKey("the"));
            Assert.False(result.ContainsKey("and"));
            Assert.False(result.ContainsKey("for"));
        }

        [Fact]
        public void CountWords_MessageWithShortWords_ReturnsWordCountsExcludingShortWords()
        {
            var message = "An error occurred e bo system.";
            var result = LogAnalyzerHelper.CountWords(message);
            Assert.Equal(1, result["error"]);
            Assert.Equal(1, result["occurred"]);
            Assert.Equal(1, result["system"]);
            Assert.False(result.ContainsKey("e"));
            Assert.False(result.ContainsKey("bo"));
        }
    }
}
