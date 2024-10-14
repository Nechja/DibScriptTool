using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace dibscript;

public interface IFileProcessor
{
    Task ProcessDibFile(string dibFilePath);
}

public class DibFileProcessor(
        IFileParser dibFileParser,
        IScriptRunner scriptRunner,
        ErrorFormatter errorFormatter,
        ILogger<DibFileProcessor> logger) : IFileProcessor
{
    public async Task ProcessDibFile(string dibFilePath)
    {
        try
        {
            var csharpCells = await dibFileParser.ExtractCSharpCells(dibFilePath);

            int cellNumber = 1;
            foreach (var cellContent in csharpCells)
            {
                logger.LogInformation($"Executing C# Cell #{cellNumber}...");
                var result = await scriptRunner.RunScript(cellContent);

                if (result.ExitCode != 0)
                {
                    logger.LogError("Error executing script:");
                    logger.LogError(errorFormatter.FormatError(result.Error));
                }
                else
                {
                    Console.WriteLine(result.Output);
                }

                cellNumber++;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the dib file.");
        }
    }
}
