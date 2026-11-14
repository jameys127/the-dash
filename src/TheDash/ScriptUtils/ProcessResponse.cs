using System.Diagnostics;
using Spectre.Console;

public class ProcessResponse
{
    public bool Started {get;}
    public Markup Response {get;}

    public ProcessResponse (bool Started, Markup Response)
    {
        this.Started = Started;
        this.Response = Response;
    }
}