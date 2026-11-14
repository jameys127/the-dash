using System.Diagnostics;
using Spectre.Console;

public class LoadingSpinner
{
    private Spinner spinner;
    private Stopwatch stopwatch = new Stopwatch();

    public LoadingSpinner(Spinner spinner)
    {
        this.spinner = spinner;
    }

    public void Start()
    {
        stopwatch.Restart();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    public string CurrentFrame()
    {
        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        double frameTime = spinner.Interval.TotalMilliseconds;

        int framesPassed = (int)(elapsedTime/frameTime);
        int index = framesPassed % spinner.Frames.Count;

        return Markup.Escape(spinner.Frames[index]);
    }
}