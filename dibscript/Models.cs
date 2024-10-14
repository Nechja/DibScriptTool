using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dibscript;

public record ParsedArgs(string FilePath, bool ShowHelp);
public record ScriptResult(string Output, string Error, int ExitCode);