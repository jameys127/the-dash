using System.Diagnostics;
using Spectre.Console;

public class ScriptRunner
{
    Process? process;

    public bool Start(Script script)
    {
        process = new Process();
        try
        {
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = script.ScriptPath;
            bool started = process.Start();
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

    // public Markup RunSelectedScript(Script script)
    // {
    //     string path = script.ScriptPath;

    //     try
    //     {
    //         using (Process runScript = new Process())
    //         {
    //             runScript.StartInfo.UseShellExecute = true;
    //             runScript.StartInfo.FileName = path;
    //             runScript.Start();
    //             return new Markup("[green]\u2713 Success:[/] Script successfully ran!");
    //         }
    //     }catch(Exception)
    //     {
    //         return new Markup($"[red]\u2717 Script Error: [/]Couldn't run script! Please try again.");
    //     }
    // }
}
