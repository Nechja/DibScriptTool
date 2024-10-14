using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace dibscript;

public interface IScriptRunner
{
    Task<ScriptResult> RunScript(string scriptContent);
}

public class CSXScriptRunner(ILogger<CSXScriptRunner> logger) : IScriptRunner
{
    public async Task<ScriptResult> RunScript(string scriptContent)
    {
        var tempScriptPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.csx");

        try
        {
            await File.WriteAllTextAsync(tempScriptPath, scriptContent);

            var processStartInfo = new ProcessStartInfo("dotnet-script", $"\"{tempScriptPath}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            return new ScriptResult(output, error, process.ExitCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running the script.");
            throw;
        }
        finally
        {
            try
            {
                if (File.Exists(tempScriptPath))
                {
                    File.Delete(tempScriptPath);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Failed to delete temporary script file: {tempScriptPath}");
            }
        }
    }
}
