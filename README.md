# Log Analyzer

A .NET console tool that parses structured log files and prints a summary report.

## Requirements

- .NET 10 SDK

## Install

Run the install script from the solution root:

```powershell
./install.ps1
```

This packs and installs the tool globally, making `log-analyzer` available in any terminal.

## Usage

```
log-analyzer <path-to-logfile>
```

**Example:**

```
log-analyzer C:\logs\app.log
```

**Exit codes:**
- `0` = success
- `2` = file not found or unreadable

## Uninstall

```powershell
dotnet tool uninstall -g log-analyzer
```

## Running Tests

```powershell
dotnet test ./log-analyzer-tests/log-analyzer-tests.csproj
```

## Design Choices & Trade-offs

### Streaming vs. reading the whole file

The program reads the file line-by-line using `StreamReader` rather than loading it all into memory at once. This keeps memory usage constant regardless of file size, since every metric in the report (level counts, most recent error, word frequencies) can be computed in as we read the lines with no need to revisit earlier lines.

### Enum declaration order drives output order

The `LogLevels` enum is declared in the exact order the spec requires (`TRACE`, `DEBUG`, `INFO`, `WARN`, `ERROR`, `FATAL`). While this makes sense from a human-readability point of view, it allows the level-count dictionary to be initialized using `Enum.GetValues<LogLevels>()`, which preserves that declaration order. This means iterating the dictionary to print the report naturally produces the correct output order with no sorting logic needed.

### HashSet for stop words

Stop words are stored in a `HashSet<string>` rather than an array. I intitalily went with an array, whose `.Contains()` is O(n) and is called for every token in every INFO message. A `HashSet` reduces that to O(1), increasing performance for larger log files.

### Compiled regex as static fields

Both regular expressions are compiled once as `static readonly` fields. `RegexOptions.Compiled` trades a small upfront cost for faster matching on repeated calls. This is valuable here since the same patterns are applied to every line in the file.

### Null as parse failure

`ParseLine` returns `null` for any line that fails to parse whether the structure doesn't match or the log level is unrecognised. This keeps the call site simple (`if (parsedLine != null)`) without needing a dedicated result type. For a larger system a `Result<T, E>` type would be more expressive and also match the patterns used in F#, but null is sufficient at this scale.
