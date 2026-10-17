using Spectre.Console;
using Spectre.Console.Rendering;

public class MainPage
{
    private bool running = true;
    private Dictionary<PageEnum, BasePage> AllPages = new Dictionary<PageEnum, BasePage>();
    private BasePage page;
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

        AnsiConsole.Live(layout)
            .Start(ctx =>
            {
                while (running)
                {
                    layout.Update(currentRender);
                    ctx.Refresh();
                    ConsoleKeyInfo keypress = Navigation.MenuNavigation();
                    RenderedPage = page.PageNavigation(keypress);
                    if(RenderedPage == PageEnum.EXIT)
                    {
                        running = false;
                        continue;
                    }
                    page = AllPages[RenderedPage];
                    currentRender = page.ReturnRender();
                    Thread.Sleep(16);
                }
            });
    }
}
