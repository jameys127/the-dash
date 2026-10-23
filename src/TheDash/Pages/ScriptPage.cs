using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

public class ScriptPage : BasePage
{
    // *** Database instance variables ***
    private Database db;
    private ScriptController scriptController;
    private ScriptRunner scriptRunner;
    private NewListItem NewScript;

    // *** Prompt boxes that popup booleans ***
    private PageMode CurrentPageMode = PageMode.LIST;

    // *** For prompt box input and design ***
    private bool confirm = false; //Selction of the confirmation prompt
    private StringBuilder input = new StringBuilder();
    private Dictionary<int, string> InputBoxHeader = new Dictionary<int, string> {
        {1, "[[ 1 / 3 ]] Enter the [bold italic]path[/] to the script"},
        {2, "[[ 2 / 3 ]] Enter the [bold italic]name[/] of the script"},
        {3, "[[ 3 / 3 ]] Enter the [bold italic]description[/] of the script"}
    };
    private Markup resultMarkup = new Markup("[green]\u2713 Success:[/] Script added!");


    // *** The list of scripts ***
    private List<Script> ListOfScripts = new List<Script>();


    // *** Menu navigation ***
    private int selection = 0;


    public ScriptPage(Database db)
    {
        this.db = db;
        scriptController = new ScriptController(db);
        scriptRunner = new ScriptRunner();
        NewScript = new NewListItem(db, scriptController);
    }


    public override PageEnum PageNavigation(ConsoleKeyInfo keypress)
    {
        switch(CurrentPageMode){
            case PageMode.LIST:
                return HandleListNav(keypress);
            case PageMode.ADDSCRIPT:
                return HandleAddNav(keypress);
            case PageMode.CONFRIM:
                return HandleConfirmNav(keypress);
            case PageMode.RESULT:
                return HandleResultNav(keypress);
            case PageMode.RUNNINGSCRIPT:
                return HandleRunningNav(keypress);
            case PageMode.UPDATE:
                return HandleUpdateNav(keypress);
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }



    public override IRenderable ReturnRender()
    {
        Layout panel = new Layout("root").SplitRows(
            new Layout("Title", new TitleRender("Scripts - Choose a script")).Size(1),
            new Layout("Body", MakeTable()),
            new Layout("Footer", new TitleRender("Press [ ESC ] to go back | Press [ + ] to add a script")).Size(1)
        );
        return panel;
    }



    public Padder MakeTable()
    {
        if(CurrentPageMode == PageMode.LIST)
        {
            ListOfScripts = scriptController.GetAllScripts();
            var table = new Table();
            table.AddColumn("[purple]Scripts[/]", col => col.Centered());
            table.AddColumn("");
            table.AddColumn("[purple]Description[/]", col => col.Centered());
            table.AddEmptyRow();
            if(ListOfScripts.Count == 0)
            {
                table.AddEmptyRow();
                table.AddRow("[purple]0 scripts found![/]", "[purple]|[/]", "[purple]Press [[ [slowblink]+[/] ]] to add a script to the list[/]");
            }
            else
            {
                for(int i = 0; i < ListOfScripts.Count; i++)
                {
                    if(selection == i)
                    {
                        table.AddRow($"[blue]>[/] [blue]{ListOfScripts[i].Name}[/]", "[blue]|[/]", $"[blue]{ListOfScripts[i].Description}[/]");
                        table.AddEmptyRow();
                        continue;
                    }
                    table.AddRow($"  [purple]{ListOfScripts[i].Name}[/]", "[purple]|[/]", $"[purple]{ListOfScripts[i].Description}[/]");
                    table.AddEmptyRow();
                }
            }

            var padder = new Padder(
                Align.Center(
                    table.Border(TableBorder.SimpleHeavy)
                        .BorderColor(Color.Purple)
                        .Expand()
                ), new Padding(10, 2, 10 ,2)
            );
            return padder;
        }else{
            return SwapPanel();
        }
    }


    public Padder SwapPanel()
    {
        var middle = new Layout();
        var answer = confirm ? "> [bold]Yes[/]        No" : "  Yes      > [bold]No[/]";

        var inputLayout = new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Left(new Markup($"[blue]{input.ToString()}[slowblink]|[/][/]"), VerticalAlignment.Middle)
                            ).Expand().Header($"{InputBoxHeader[NewScript.InputStep]}").HeaderAlignment(Justify.Center).RoundedBorder().BorderColor(Color.Blue)
                        );

        var resultLayout = new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Center(new Rows(
                                    resultMarkup,
                                    new Text(""),
                                    new Markup("[blue]> [/][bold blue] OK [/]")
                                ), VerticalAlignment.Middle)
                            ).Expand().RoundedBorder().BorderColor(Color.Blue)
                        );
        var confirmationLayout = new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Center(new Rows(
                                    new Markup($"[yellow]\u26a0[/] Are you sure you want to delete this script?"),
                                    new Text (""),
                                    new Markup(answer)
                                ), VerticalAlignment.Middle)
                            ).Expand().RoundedBorder().BorderColor(Color.Blue)
                        );
        var runningScriptLayout = new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Center(new Rows(
                                    new Text(""),
                                    new Markup("[bold blue]Script is running.. [/]"),
                                    new Text("")
                                ), VerticalAlignment.Middle)
                            ).Expand().RoundedBorder().BorderColor(Color.Blue)
                        );

        switch(CurrentPageMode){
            case PageMode.ADDSCRIPT:
                middle = inputLayout;
                break;
            case PageMode.RESULT:
                middle = resultLayout;
                break;
            case PageMode.CONFRIM:
                middle = confirmationLayout;
                break;
            case PageMode.UPDATE:
                middle = inputLayout;
                break;
            case PageMode.RUNNINGSCRIPT:
                middle = runningScriptLayout;
                break;
        }

        return new Padder(new Layout("BodyRoot")
            .SplitRows(
                new Layout("UnusedTop").Ratio(3).Update(new Text("")),
                // ^^ Simply blank space ^^

                new Layout("MiddleRow").Size(5)
                    .SplitColumns(
                        new Layout("UnusedLeft").Ratio(1).Update(new Text("")),
                        // ^^ Simply blank space ^^

                        middle,

                        // vv Simply blank space vv
                        new Layout("UnusedRight").Ratio(1).Update(new Text(""))
                    ),

                // vv Simply blank space vv
                new Layout("UnusedBottom").Ratio(3).Update(new Text(""))
            ), new Padding(0,0,0,0));
    }

    private PageEnum HandleListNav(ConsoleKeyInfo keypress){
        switch(keypress){
            case {Key : ConsoleKey.UpArrow} when ListOfScripts.Count > 0:
                if(selection == 0){
                    StateHasChanged = true;
                    selection = ListOfScripts.Count - 1;
                    return PageEnum.SCRIPTS;
                }else{
                    selection--;
                    StateHasChanged = true;
                    return PageEnum.SCRIPTS;
                }
            case {Key : ConsoleKey.DownArrow} when ListOfScripts.Count > 0:
                if(selection == ListOfScripts.Count - 1){
                    StateHasChanged = true;
                    selection = 0;
                    return PageEnum.SCRIPTS;
                }else{
                    selection++;
                    StateHasChanged = true;
                    return PageEnum.SCRIPTS;
                }
            case {Key : ConsoleKey.Delete} when ListOfScripts.Count > 0:
                StateHasChanged = true;
                CurrentPageMode = PageMode.CONFRIM;
                return PageEnum.SCRIPTS;
            case {Key : ConsoleKey.Escape}:
                StateHasChanged = true;
                selection = 0;
                return PageEnum.MENU;
            case {Key : ConsoleKey.Enter} when ListOfScripts.Count > 0:
                StateHasChanged = true;
                bool didItStart = RunScript();
                if(didItStart == false){
                    resultMarkup = new Markup("[red]\u2717 Script Error:[/] Failed to start!");
                    CurrentPageMode = PageMode.RESULT;
                    return PageEnum.SCRIPTS;
                }else{
                    CurrentPageMode = PageMode.RUNNINGSCRIPT;
                    return PageEnum.SCRIPTS;
                }
            case {KeyChar : '+'}:
                CurrentPageMode = PageMode.ADDSCRIPT;
                StateHasChanged = true;
                NewScript = new NewListItem(db, scriptController);
                return PageEnum.SCRIPTS;
            case {KeyChar : 'U'} when ListOfScripts.Count > 0:
                input.Append(ListOfScripts[selection].ScriptPath);
                CurrentPageMode = PageMode.UPDATE;
                StateHasChanged = true;
                return PageEnum.SCRIPTS;
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }

    private PageEnum HandleAddNav(ConsoleKeyInfo keypress){
        switch (keypress){
            case {Key : ConsoleKey.Backspace} when input.Length > 0:
                StateHasChanged = true;
                input.Remove(input.Length - 1, 1);
                return PageEnum.SCRIPTS;
            case {Key : ConsoleKey.Enter} when input.Length > 0:
                StateHasChanged = true;
                Markup? result = NewScript.AddToList(input.ToString());
                if(result == null){
                    input.Clear();
                    return PageEnum.SCRIPTS;
                }else{
                    resultMarkup = result;
                    CurrentPageMode = PageMode.RESULT;
                    return PageEnum.SCRIPTS;
                }
            case {KeyChar : char c} when !char.IsControl(c):
                StateHasChanged = true;
                input.Append(keypress.KeyChar);
                return PageEnum.SCRIPTS;
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }

    private PageEnum HandleConfirmNav(ConsoleKeyInfo keypress){
        switch(keypress){
            case {Key : ConsoleKey.RightArrow}:
                StateHasChanged = true;
                confirm = !confirm;
                return PageEnum.SCRIPTS;
            case {Key : ConsoleKey.LeftArrow}:
                StateHasChanged = true;
                confirm = !confirm;
                return PageEnum.SCRIPTS;
            case {Key : ConsoleKey.Enter}:
                StateHasChanged = true;
                confirm = false;
                resultMarkup = RemoveScript(ListOfScripts[selection].Id);
                CurrentPageMode = PageMode.LIST;
                return PageEnum.SCRIPTS;
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }

    private PageEnum HandleResultNav(ConsoleKeyInfo keypress){
        switch(keypress){
            case {Key :  ConsoleKey.Enter}:
                input.Clear();
                selection = 0;
                resultMarkup = new Markup("");
                StateHasChanged = true;
                CurrentPageMode = PageMode.LIST;
                return PageEnum.SCRIPTS;
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }
    
    private PageEnum HandleRunningNav(ConsoleKeyInfo keypress){
        if(scriptRunner.IsScriptRunning()){
            return PageEnum.SCRIPTS;
        }else{
            resultMarkup = ScriptResult();
            CurrentPageMode = PageMode.RESULT;
            return PageEnum.SCRIPTS;
        }
    }
    private PageEnum HandleUpdateNav(ConsoleKeyInfo keypress){
        switch(keypress){
            case {Key : ConsoleKey.Backspace} when input.Length > 0:
                StateHasChanged = true;
                input.Remove(input.Length - 1, 1);
                return PageEnum.SCRIPTS;
            case {Key : ConsoleKey.Enter} when input.Length > 0:
                StateHasChanged = true;
                Markup? result = NewScript.AddToList(input.ToString());
                if(result == null){
                    input.Clear();
                    if(NewScript.InputStep == 2){
                        input.Append(ListOfScripts[selection].Name);
                    }else{
                        input.Append(ListOfScripts[selection].Description);
                    }
                    return PageEnum.SCRIPTS;
                }else{
                    resultMarkup = result;
                    CurrentPageMode = PageMode.RESULT;
                    return PageEnum.SCRIPTS;
                }
            case {KeyChar : char c} when !char.IsControl(c):
                StateHasChanged = true;
                input.Append(keypress.KeyChar);
                return PageEnum.SCRIPTS;
        }
        StateHasChanged = false;
        return PageEnum.SCRIPTS;
    }



    private Markup RemoveScript(int i)
    {
        var result = scriptController.DeleteScript(i);
        return result;
    }


    private bool RunScript()
    {
        scriptRunner = new ScriptRunner();
        return scriptRunner.Start(ListOfScripts[selection]);
    }

    private Markup ScriptResult()
    {
        return scriptRunner.Result();
    }

    public override bool UpdateNeeded()
    {
        return StateHasChanged;
    }
}


public enum PageMode{
    LIST,
    ADDSCRIPT,
    RESULT,
    CONFRIM,
    UPDATE,
    RUNNINGSCRIPT
}



