using Spectre.Console;
using Spectre.Console.Rendering;

public class MainPage
{
    private bool running = true;
    private Dictionary<PageEnum, BasePage> AllPages = new Dictionary<PageEnum, BasePage>();
    private BasePage page;
    private int lastWidth = Console.WindowWidth;
    private int lastHeight = Console.WindowHeight;
    private IRenderable currentRender;
    private PageEnum RenderedPage;

    public MainPage(Database db)
    {
        MenuPage menu = new MenuPage(db);
	    ScriptPage script = new ScriptPage(db);
        AllPages.Add(PageEnum.MENU, menu);
	    AllPages.Add(PageEnum.SCRIPTS, script);
        RenderedPage = PageEnum.MENU;
        page = AllPages[RenderedPage];
        currentRender = page.ReturnRender();
    }


    public void StartDash()
    {
        Layout layout = new Layout("thedash");

        AnsiConsole.AlternateScreen(() =>{
            AnsiConsole.Live(layout)
                .Start(ctx =>
                {
                    layout.Update(currentRender);
                    ctx.Refresh();
                    while (running)
                    {
                        ConsoleKeyInfo keypress = Navigation.MenuNavigation();
                        RenderedPage = page.PageNavigation(keypress);
                        if (page.UpdateNeeded() || (lastHeight != Console.WindowHeight || lastWidth != Console.WindowWidth))
                        {
                            lastHeight = Console.WindowHeight;
                            lastWidth = Console.WindowWidth;
                            if(RenderedPage == PageEnum.EXIT)
                            {
                                running = false;
                                continue;
                            }
                            page = AllPages[RenderedPage];
                            currentRender = page.ReturnRender();
                            layout.Update(currentRender);
                            ctx.Refresh();
                        }
                        Thread.Sleep(16);
                    }
                });
        });
    }
}
