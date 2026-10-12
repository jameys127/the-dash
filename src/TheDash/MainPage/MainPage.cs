using Spectre.Console;
using Spectre.Console.Rendering;

public class MainPage
{
    bool running = true;
    Dictionary<PageEnum, BasePage> AllPages = new Dictionary<PageEnum, BasePage>();
    BasePage page;
    Panel currentPanel;
    PageEnum RenderedPage;

    public MainPage()
    {
        MenuPage menu = new MenuPage();
	ScriptPage script = new ScriptPage();
        AllPages.Add(PageEnum.MENU, menu);
	AllPages.Add(PageEnum.SCRIPTS, script);
        RenderedPage = PageEnum.MENU;
        page = AllPages[RenderedPage];
        currentPanel = page.ReturnPanel();
    }


    public void StartDash()
    {
        Layout layout = new Layout("thedash");

        AnsiConsole.Live(layout)
            .Start(ctx =>
            {
                while (running)
                {
                    layout.Update(currentPanel);
                    ctx.Refresh();
                    ConsoleKey keypress = Navigation.MenuNavigation();
                    RenderedPage = page.PageNavigation(keypress);
                    if(RenderedPage == PageEnum.EXIT)
                    {
                        running = false;
                        continue;
                    }
                    page = AllPages[RenderedPage];
                    currentPanel = page.ReturnPanel();
                    Thread.Sleep(16);
                }
            });
    }
}
