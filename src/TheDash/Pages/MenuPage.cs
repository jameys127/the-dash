using Spectre.Console;
using Spectre.Console.Rendering;

public class MenuPage : BasePage
{
    List<string> MenuItems = new List<string> { "Scripts", "Notes", "Game", "Exit"};
    int menuSelection = 0;

    public override PageEnum PageNavigation(ConsoleKey key)
    {
        if(key == ConsoleKey.UpArrow)
        {
            menuSelection = menuSelection == 0 ? menuSelection = MenuItems.Count-1 : menuSelection = menuSelection - 1;
            return PageEnum.MENU;
        }else if(key == ConsoleKey.DownArrow)
        {
            menuSelection = menuSelection == MenuItems.Count-1 ? menuSelection = 0 : menuSelection = menuSelection + 1;
            return PageEnum.MENU;
        }
        else if(key == ConsoleKey.Enter)
        {
            if(menuSelection == MenuItems.Count - 1)
            {
                return PageEnum.EXIT;
            }
            return PageEnum.MENU;
        }
        else
        {
            return PageEnum.MENU;
        }
    }

    public override Panel ReturnPanel()
    {
        var rows = new List<IRenderable>
        {
            new FigletText(Fonts.font3d, "The Dash").Color(Color.Purple).Smushed(),
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
}