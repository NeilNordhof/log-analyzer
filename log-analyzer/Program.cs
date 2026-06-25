using log_analyzer;
using log_analyzer.Types;

if (args.Length == 0)
{
    Console.WriteLine("Error: No file path provided.");
    return 2;
}

string filePath = args[0]; 

if (!File.Exists(filePath))
{
    Console.WriteLine("Error: File does not exist.");
    return 2;
}

// Open a stream reader for the file and read it line by line
using var reader = new StreamReader(filePath);

var logReport = new LogReport();

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line == null)
    {
        continue;
    }
    logReport.TotalLines++;

    var parsedLine = LogAnalyzerHelper.ParseLine(line);
    if (parsedLine != null)
    {
        logReport.LogLevelCounts[parsedLine.LogLevel]++;
               

        if (parsedLine.LogLevel == LogLevels.INFO)
        {
            var wordCounts = LogAnalyzerHelper.CountWords(parsedLine.Message);

            foreach ((string word, int count) in wordCounts)
            {
                logReport.WordOccurences[word] = logReport.WordOccurences.GetValueOrDefault(word) + count;
            }
        }

        if (parsedLine.LogLevel == LogLevels.ERROR
            && parsedLine.TimeStamp >= logReport.LastErrorDateTime)
        {
            logReport.LastErrorDateTime = parsedLine.TimeStamp;
            logReport.LastErrorMessage = parsedLine.Message;
        }

    }
    else
    {
        logReport.MalformedCount++;
    }
}

Console.WriteLine($"Total Entries: {logReport.TotalLines}");
foreach((LogLevels logLevel, int count) in logReport.LogLevelCounts)
{
    Console.WriteLine($"{logLevel}: {count}");
}
Console.WriteLine($"Malformed: {logReport.MalformedCount}");
Console.WriteLine($"Most Recent ERROR: {(string.IsNullOrEmpty(logReport.LastErrorMessage) ? "N/A" : logReport.LastErrorMessage)}");
var top3Words =logReport.WordOccurences.OrderByDescending(kvp => kvp.Value)
    .ThenBy(kvp => kvp.Key)
    .Take(3)
    .ToList();

Console.WriteLine($"Top 3 Frequent Words (INFO): {string.Join(", ", top3Words.Select(kvp => kvp.Key))}");

return 0;