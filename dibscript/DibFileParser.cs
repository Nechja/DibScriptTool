using Microsoft.Extensions.Logging;

namespace dibscript;

public interface IFileParser
{
    Task<List<string>> ExtractCSharpCells(string filePath);
}

public class DibFileParser(ILogger<DibFileParser> logger) : IFileParser
{
    public async Task<List<string>> ExtractCSharpCells(string filePath)
    {
        var csharpCells = new List<string>();

        try
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            bool inCSharpBlock = false;
            var currentCell = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith("#!"))
                {
                    if (inCSharpBlock)
                    {
                        csharpCells.Add(string.Join(Environment.NewLine, currentCell));
                        currentCell.Clear();
                        inCSharpBlock = false;
                    }

                    if (line.StartsWith("#!csharp"))
                    {
                        inCSharpBlock = true;
                    }
                }
                else if (inCSharpBlock)
                {
                    currentCell.Add(line);
                }
            }

            if (inCSharpBlock && currentCell.Count > 0)
            {
                csharpCells.Add(string.Join(Environment.NewLine, currentCell));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to extract C# cells from {filePath}");
            throw;
        }

        return csharpCells;
    }
}
