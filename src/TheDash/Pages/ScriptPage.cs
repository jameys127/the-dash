using Spectre.Console;

public class ScriptPage : BasePage
{
    public override PageEnum PageNavigation(ConsoleKey key)
    {
        return PageEnum.SCRIPTS;
    }

    public override Panel ReturnPanel()
    {
        Panel panel = new Panel(
		new Rows(new TitleRender("Scripts - Choose a script to run"))
	)
            .BorderColor(Color.Purple)
	    .Padding(0, 0, 0, 0)
            .NearBorder()
            .Expand();
        return panel;
    }
}
