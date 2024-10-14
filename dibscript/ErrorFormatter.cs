using Microsoft.Extensions.Logging;


namespace dibscript;
public class ErrorFormatter(ILogger<ErrorFormatter> logger)
{
    public string FormatError(string error)
    {
        var lines = error.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var filteredLines = lines.Where(line => !line.TrimStart().StartsWith("at ")).ToList();
        return string.Join(Environment.NewLine, filteredLines);
    }
}