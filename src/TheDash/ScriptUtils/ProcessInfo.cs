using System.Diagnostics;

public class ProcessInfo{
    private record TerminalProfile(string EnvironmentVariable, string FileName, string Prefix);
    private TerminalProfile[] Terminals = {
        new ("KITTY_WINDOW_ID", "kitty", ""),
        new ("ALACRITTY_WINDOW_ID", "alacritty", "-e"),
        new ("KONSOLE_VERSION", "konsole", "-e"),
        new ("GHOSTTY_RESOURCES_DIR", "ghostty", "-e")
    };


    public Process MakeProcess(Script script){
        Process process = new Process();
        ScriptFileInfo parsedScript = ParseScript(script);
        if(OperatingSystem.IsWindows()){
            switch(parsedScript.ScriptType){
                case TypeOfScript.POWERSHELL:
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.WorkingDirectory = parsedScript.ScriptPath;
                    process.StartInfo.Arguments = $"-ExecutionPolicy ByPass -File \"{parsedScript.ScriptFile}\"";
                    return process;
                case TypeOfScript.PYTHON:
                    process.StartInfo.FileName = "python.exe";
                    process.StartInfo.WorkingDirectory = parsedScript.ScriptPath;
                    process.StartInfo.Arguments = $"\"{parsedScript.ScriptFile}\"";
                    return process;
                case TypeOfScript.BATCH:
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WorkingDirectory = parsedScript.ScriptPath;
                    process.StartInfo.Arguments = $"/c \"{parsedScript.ScriptFile}\"";
                    return process;
                default:
                    process.StartInfo.FileName = script.ScriptPath;
                    return process;
            }
        }else{
            process = FindTerminal(process);
            process.StartInfo.WorkingDirectory = parsedScript.ScriptPath;
            process.StartInfo.ArgumentList.Add("./" + parsedScript.ScriptFile);
            return process;

        }
    }

    public Process FindTerminal(Process process){
        foreach (var env in Terminals){
            if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(env.EnvironmentVariable))){
                process.StartInfo.FileName = env.FileName;
                if(env.Prefix != ""){
                    process.StartInfo.ArgumentList.Add(env.Prefix);
                }
                return process;
            }
        }
        throw new Exception("Unsupported Terminal");
    }

    public ScriptFileInfo ParseScript(Script script){
        string fileExtension = Path.GetExtension(script.ScriptPath);
        string fileName = Path.GetFileName(script.ScriptPath);
        string? filePath = Path.GetDirectoryName(script.ScriptPath);
        if(filePath != null){
            return new ScriptFileInfo(fileExtension, fileName, filePath);
        }else{
            throw new Exception("Path directory name error");
        }
    }
}

