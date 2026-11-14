using System.Diagnostics;
using System.Text;
using Spectre.Console;

public class ScriptRunner
{
    Process? process;

    public ProcessResponse Start(Script script)
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
                return new ProcessResponse(true, new Markup("Success"));
            }
            else
            {
                process.Dispose();
                process = null;
                return new ProcessResponse(false, new Markup("[red]\u2717 Script Error: [/]Script failed to start!"));
            }
            
        }
        catch(Exception e)
        {
            process?.Dispose();
            process = null;
            return new ProcessResponse(false, new Markup($"[red]\u2717 Script Error: [/]{Markup.Escape(e.Message)}"));
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
            process.Dispose();
            process = null;
            return new Markup ($"[green]\u2713 Success:[/] No issues!");            
        }
    }
}
    

