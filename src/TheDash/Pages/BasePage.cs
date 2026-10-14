using Spectre.Console;

public abstract class BasePage
{
    public abstract PageEnum PageNavigation(ConsoleKeyInfo keypress);
    public abstract Panel ReturnPanel();
}