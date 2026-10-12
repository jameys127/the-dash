using Spectre.Console;

public class ScriptPage : BasePage
{
    public override PageEnum PageNavigation(ConsoleKey key)
    {
        return PageEnum.SCRIPTS;
    }

    public override Panel ReturnPanel()
    {
        Panel panel = new Panel("Stuff")
            .Header("The Dash")
            .BorderColor(Color.Purple)
            .RoundedBorder()
            .Expand();
        return panel;
    }
}
