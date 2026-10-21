using Spectre.Console;
using Spectre.Console.Rendering;

public abstract class BasePage
{
    public bool StateHasChanged;
    public abstract PageEnum PageNavigation(ConsoleKeyInfo keypress);
    public abstract IRenderable ReturnRender();
    public abstract bool UpdateNeeded();
}