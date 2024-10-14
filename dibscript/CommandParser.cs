namespace dibscript;

public class CommandParser
{
    public ParsedArgs ParseArgs(string[] args)
    {
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
        {
            return new ParsedArgs(null, true);
        }

        return new ParsedArgs(args[0], false);
    }

    public void DisplayHelp()
    {
        Console.WriteLine("Usage: dib-runner <path to .dib file>");
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help    Show this help message");
    }
}
