using System.Diagnostics;
using System.Text;
using Spectre.Console;

public class ScriptRunner
{
    Process? process;

    public bool Start(Script script)
    {
        try
        {
            bool started;
            process = new ProcessInfo().MakeProcess(script);
            if(OperatingSystem.IsWindows()){
                process.StartInfo.UseShellExecute = true;
                started = process.Start();
            }else{
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                started = process.Start();
            }
            if (started)
            {
                return true;
            }
            else
            {
                process.Dispose();
                process = null;
                return false;
            }
            
        }
        catch(Exception)
        {
            process?.Dispose();
            process = null;
            return false;
        }
    }

    public bool IsScriptRunning()
    {
        if(process == null)
        {
            return false;
        }
        else
        {
            return !process.HasExited;
        }
    }

    public Markup Result()
    {
        if(process == null)
        {
            return new Markup($"[red]\u2717 Script Error: [/]Script never started!");
        }
        else
        {
            int exitCode = process.ExitCode;
            process.Dispose();
            process = null;
            return new Markup ($"[green]\u2713 Success:[/] Finished with exit code {exitCode}");            
        }
    }
    
    public ScriptFileInfo ParsePath(Script script){
        string input = script.ScriptPath;
        string type = "";
        string file = "";
        string path = "";
        int i = input.Length - 1;
        StringBuilder inputBuilder = new StringBuilder();
        while(input[i] != '/' && input[i] != '\\'){
           inputBuilder.Insert(0, input[i]);
           if(input[i] == '.'){
                type = inputBuilder.ToString();
           }
           i--;
        }
        file = inputBuilder.ToString();
        inputBuilder.Clear();
        while(i >=0){
            inputBuilder.Insert(0, input[i]);
            i--;
        }
        path = inputBuilder.ToString();
        ScriptFileInfo parsed = new ScriptFileInfo(type, file, path);
        return parsed;
    }
}
