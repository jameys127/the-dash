using Spectre.Console;
using Spectre.Console.Rendering;

public class MainPage
{
    bool running = true;
    List<string> MenuItems = new List<string> { "Scripts", "Notes", "Game", "Exit"};
    Panel currentPanel = new Panel("inital");
    public void StartDash()
    {
        Layout layout = new Layout("thedash");
        int menuSelection = 0;

        AnsiConsole.Live(layout)
            .Start(ctx =>
            {
                while (running)
                {
                    currentPanel = RenderPanel(menuSelection);
                    layout.Update(currentPanel);
                    ctx.Refresh();
                    ConsoleKey keypress = Navigation.MenuNavigation();
                    if(keypress == ConsoleKey.UpArrow)
                    {
                    	menuSelection = menuSelection == 0 ? menuSelection = MenuItems.Count-1 : menuSelection = menuSelection - 1;
                    }else if(keypress == ConsoleKey.DownArrow)
                    {
                        menuSelection = menuSelection == MenuItems.Count-1 ? menuSelection = 0 : menuSelection = menuSelection + 1;	
                    }
                    else if(keypress == ConsoleKey.Enter)
                    {
                        if(menuSelection == MenuItems.Count - 1)
                        {
                            running = false;
                            continue;
                        }
                        currentPanel = Enter(menuSelection);
                    }
                    Thread.Sleep(16);
                }
            });
    }

    public Panel RenderPanel(int selection)
    {
        var rows = new List<IRenderable>
        {
            new FigletText(Fonts.font3d, "The Dash").Color(Color.Purple).Smushed(),
            new Text("")
        };

        for(int i = 0; i < MenuItems.Count; i++)
        {
            if(selection == i)
            {
                rows.Add(new FigletText(Fonts.fontminiwi, "> " + MenuItems[i]).Color(Color.Blue));
            }
            else
            {
                rows.Add(new FigletText(Fonts.fontminiwi, MenuItems[i]).Color(Color.Purple));
            }
        }
        Panel panel = new Panel(Align.Center(new Rows(rows), VerticalAlignment.Middle))
            .Header("The Dash")
            .BorderColor(Color.Purple)
            .RoundedBorder()
            .Expand();
        return panel;
    }


    public Panel Enter(int selection)
    {
        if(selection == MenuItems.Count - 1)
        {
            running = false;
            return new Panel("exit");

        }
        return new Panel("nothing");
    }
}