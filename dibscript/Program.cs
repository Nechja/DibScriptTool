using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace dibscript;
class Program
{
    static async Task Main(string[] args)
    {
        CheckIfDotNetScriptIsInstalled();
        using IHost host = CreateHostBuilder(args).Build();

        var commandParser = host.Services.GetRequiredService<CommandParser>();
        var dibFileProcessorService = host.Services.GetRequiredService<IFileProcessor>();

        var (dibFilePath, showHelp) = commandParser.ParseArgs(args);

        if (showHelp || string.IsNullOrWhiteSpace(dibFilePath))
        {
            commandParser.DisplayHelp();
            return;
        }

        await dibFileProcessorService.ProcessDibFile(dibFilePath);
        await host.StopAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddLogging(configure => configure.AddConsole());

                services.AddTransient<CommandParser>();
                services.AddTransient<ErrorFormatter>();
                services.AddTransient<IFileProcessor, DibFileProcessor>();
                services.AddTransient<IFileParser, DibFileParser>();
                services.AddTransient<IScriptRunner, CSXScriptRunner>();

            });

    private static void CheckIfDotNetScriptIsInstalled() {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet-script",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("dotnet-script is required but not installed.");
        }


    }

}
