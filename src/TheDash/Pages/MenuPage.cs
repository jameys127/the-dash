using Spectre.Console;
using Spectre.Console.Rendering;

public class MenuPage : BasePage
{
    
    private List<string> MenuItems = new List<string> { "Scripts", "Notes", "Game", "Exit"};
    private int menuSelection = 0;
    private Database db;
    public MenuPage(Database db)
    {
        this.db = db;
    }

    public override PageEnum PageNavigation(ConsoleKeyInfo keypress)
    {
        if(keypress.Key == ConsoleKey.UpArrow)
        {
            menuSelection = menuSelection == 0 ? menuSelection = MenuItems.Count-1 : menuSelection = menuSelection - 1;
            StateHasChanged = true;
            return PageEnum.MENU;
        }

        else if(keypress.Key == ConsoleKey.DownArrow)
        {
            menuSelection = menuSelection == MenuItems.Count-1 ? menuSelection = 0 : menuSelection = menuSelection + 1;
            StateHasChanged = true;
            return PageEnum.MENU;
        }

        else if(keypress.Key == ConsoleKey.Enter)
        {
            if(menuSelection == MenuItems.Count - 1)
            {
                StateHasChanged = true;
                return PageEnum.EXIT;
            }
	        else if(menuSelection == 0)
	        {
                StateHasChanged = true;
		        return PageEnum.SCRIPTS;
	        }
            StateHasChanged = false;
            return PageEnum.MENU;
        }
        else if(keypress.Key == ConsoleKey.Escape)
        {
            StateHasChanged = true;
            return PageEnum.EXIT;
        }
        else
        {
            StateHasChanged = false;
            return PageEnum.MENU;
        }
    }

    public override IRenderable ReturnRender()
    {
        var rows = new List<IRenderable>
        {
            new FigletText(Fonts.font3d, "The Dash").Color(Color.Purple),
            new Text("")
        };

        for(int i = 0; i < MenuItems.Count; i++)
        {
            if(menuSelection == i)
            {
                rows.Add(new FigletText(Fonts.fontminiwi, "> " + MenuItems[i]).Color(Color.Blue));
            }
            else
            {
                rows.Add(new FigletText(Fonts.fontminiwi, "  " + MenuItems[i]).Color(Color.Purple));
            }
        }
        Layout panel = new Layout(new Rows(Align.Center(new Rows(rows), VerticalAlignment.Middle)));
        return panel;
    }

    public override bool UpdateNeeded()
    {
        return StateHasChanged;
    }
}
