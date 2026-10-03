using System.Linq.Expressions;
using Spectre.Console;

namespace TheDash;
class Dash{

	//THese are the fonts for FIGlet text that need to be loaded.
	static FigletFont font3d = FigletFont.Load("src/TheDash/Assets/3d.flf");
	static FigletFont fontminiwi = FigletFont.Load("src/TheDash/Assets/miniwi.flf");
	static FigletFont fonttermdots = FigletFont.Load("src/TheDash/Assets/terminus_dots.flf");
	static FigletFont fontterm = FigletFont.Load("src/TheDash/Assets/terminus.flf");


	//Dictionaries for the different panels. Don't know if I will use one of them just yet.
	Dictionary<int, string> menuItems = new Dictionary<int, string> { {1, "Scripts"}, {2, "Notes"}, {3, "Game"}, {4, "Exit"}};
	Dictionary<int, Panel> menuItemPanels = new Dictionary<int, Panel>();


	static void Main()
	{
		// The layout that fills the screen of the current terminal
		Layout layout = new Layout("thedash");
		bool running = true;
		int menuSelection = 1;

		Dash dash = new Dash();

		AnsiConsole.Live(layout)
			.Start(ctx =>
			{
				while(running)
				{
					layout.Update(dash.RenderPanel(menuSelection));
					ctx.Refresh();
					if (Console.KeyAvailable)
					{
						var keypress = Console.ReadKey(true);
						if(keypress.Key == ConsoleKey.UpArrow)
						{
							menuSelection = menuSelection == 1 ? menuSelection = 4 : menuSelection = menuSelection - 1;
						}else if(keypress.Key == ConsoleKey.DownArrow)
						{
							menuSelection = menuSelection == 4 ? menuSelection = 1 : menuSelection = menuSelection + 1;	
						}
						else if(keypress.Key == ConsoleKey.Enter && menuSelection == 4)
						{
							running = false;
						}
					}
					Thread.Sleep(16);
				}
			});
	}

	public Panel RenderPanel(int i)
	{
		switch (i)
		{
			case 1:
				return new Panel(Align.Center(new Rows(
					new FigletText(font3d, "The Dash").Color(Color.Purple).LayoutMode(FigletLayoutMode.Smushed),
					new Text(""),
					new FigletText(fontminiwi, "> Script").Color(Color.Blue),
					new FigletText(fontminiwi, "Notes").Color(Color.Purple),
					new FigletText(fontminiwi, "Game").Color(Color.Purple),
					new FigletText(fontminiwi, "Exit").Color(Color.Purple)
				), VerticalAlignment.Middle
				))
					.Header("The Dash")
					.BorderColor(Color.Purple)
					.RoundedBorder()
					.Expand();
			case 2:
				return new Panel(Align.Center(new Rows(
					new FigletText(font3d, "The Dash").Color(Color.Purple).LayoutMode(FigletLayoutMode.Smushed),
					new Text(""),
					new FigletText(fontminiwi, "Script").Color(Color.Purple),
					new FigletText(fontminiwi, "> Notes").Color(Color.Blue),
					new FigletText(fontminiwi, "Game").Color(Color.Purple),
					new FigletText(fontminiwi, "Exit").Color(Color.Purple)
				), VerticalAlignment.Middle
				))
					.Header("The Dash")
					.BorderColor(Color.Purple)
					.RoundedBorder()
					.Expand();
			case 3:
				return new Panel(Align.Center(new Rows(
					new FigletText(font3d, "The Dash").Color(Color.Purple).LayoutMode(FigletLayoutMode.Smushed),
					new Text(""),
					new FigletText(fontminiwi, "Script").Color(Color.Purple),
					new FigletText(fontminiwi, "Notes").Color(Color.Purple),
					new FigletText(fontminiwi, "> Game").Color(Color.Blue),
					new FigletText(fontminiwi, "Exit").Color(Color.Purple)
				), VerticalAlignment.Middle
				))
					.Header("The Dash")
					.BorderColor(Color.Purple)
					.RoundedBorder()
					.Expand();
			case 4:
				return new Panel(Align.Center(new Rows(
					new FigletText(font3d, "The Dash").Color(Color.Purple).LayoutMode(FigletLayoutMode.Smushed),
					new Text(""),
					new FigletText(fontminiwi, "Script").Color(Color.Purple),
					new FigletText(fontminiwi, "Notes").Color(Color.Purple),
					new FigletText(fontminiwi, "Game").Color(Color.Purple),
					new FigletText(fontminiwi, "> Exit").Color(Color.Blue)
				), VerticalAlignment.Middle
				))
					.Header("The Dash")
					.BorderColor(Color.Purple)
					.RoundedBorder()
					.Expand();
			default:
				return new Panel("Should not have gotten this panel ;()");
				
		}		
	}
}	
