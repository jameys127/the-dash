using Spectre.Console;

public abstract class BasePage
{
    public abstract PageEnum PageNavigation(ConsoleKey key);
    public abstract Panel ReturnPanel();
}