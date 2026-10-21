using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Diagnostics;

public class ScriptPage : BasePage
{
    // *** Database instance variables ***
    private Database db;
    private ScriptController scriptController;


    // *** Prompt boxes that popup booleans ***
    private bool AddTextBoxOpen = false;
    private bool ResultTextBoxOpen = false;
    private bool ConfirmationBoxOpen = false;
    private bool UpdateTextBoxOpen = false;


    // *** For prompt box input and design ***
    private int InputBoxStep = 1; //Input step during the creation of a new script
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


    // *** Temp variables for creating a new script ***
    private string path = "";
    private string name = "";
    private string description = "";


    // *** Menu navigation ***
    private int selection = 0;


    public ScriptPage(Database db)
    {
        this.db = db;
        scriptController = new ScriptController(db);
    }


    public override PageEnum PageNavigation(ConsoleKeyInfo keypress)
    {
        if(keypress.Key == ConsoleKey.Escape && (AddTextBoxOpen || UpdateTextBoxOpen))
        {
            StateHasChanged = true;
            AddTextBoxOpen = false;
            UpdateTextBoxOpen = false;
            input.Clear();
            return PageEnum.SCRIPTS;
        }
        else if(keypress.Key == ConsoleKey.Escape)
        {
            StateHasChanged = true;
            return PageEnum.MENU;
        }

        if(keypress.Key == ConsoleKey.UpArrow)
        {
            if(ListOfScripts.Count == 0)
            {
                StateHasChanged = false;
                return PageEnum.SCRIPTS;
            }
            if(selection == 0)
            {
                StateHasChanged = true;
                selection = ListOfScripts.Count - 1;
                return PageEnum.SCRIPTS;
            }
            else
            {
                StateHasChanged = true;
                selection--;
                return PageEnum.SCRIPTS;
            }
        }
        else if(keypress.Key == ConsoleKey.DownArrow)
        {
            if(ListOfScripts.Count == 0)
            {
                StateHasChanged = false;
                return PageEnum.SCRIPTS;
            }
            if(selection == ListOfScripts.Count - 1)
            {
                StateHasChanged = true;
                selection = 0;
                return PageEnum.SCRIPTS;
            }
            else
            {
                StateHasChanged = true;
                selection++;
                return PageEnum.SCRIPTS;
            }
        }

        if (AddTextBoxOpen)
        {
            if (!char.IsControl(keypress.KeyChar))
            {
                StateHasChanged = true;
                input.Append(keypress.KeyChar);
                return PageEnum.SCRIPTS;
            }else if(input.Length != 0 && keypress.Key == ConsoleKey.Backspace)
            {
                StateHasChanged = true;
                input.Remove(input.Length - 1, 1);
                return PageEnum.SCRIPTS;
            }
            else if(input.Length != 0 && keypress.Key == ConsoleKey.Enter)
            {
                switch (InputBoxStep)
                {
                    case 1:
                        StateHasChanged = true;
                        path = input.ToString();
                        input.Clear();
                        InputBoxStep++;
                        return PageEnum.SCRIPTS;
                    case 2:
                        StateHasChanged = true;
                        name = input.ToString();
                        input.Clear();
                        InputBoxStep++;
                        return PageEnum.SCRIPTS;
                    case 3:
                        StateHasChanged = true;
                        description = input.ToString();
                        input.Clear();
                        InputBoxStep = 1;
                        resultMarkup = AddScript();
                        AddTextBoxOpen = false;
                        ResultTextBoxOpen = true;
                        return PageEnum.SCRIPTS;
                    default:
                        throw new Exception("Not possible to get here");
                }
            }
            StateHasChanged = false;
            return PageEnum.SCRIPTS;
        }

        if (ResultTextBoxOpen)
        {
            if(keypress.Key == ConsoleKey.Enter)
            {
                StateHasChanged = true;
                path = "";
                name = "";
                description = "";
                resultMarkup = new Markup("");
                selection = 0;
                ResultTextBoxOpen = false;
                return PageEnum.SCRIPTS;
            }
            StateHasChanged = false;
            return PageEnum.SCRIPTS;
        }

        if (ConfirmationBoxOpen)
        {
            if(keypress.Key == ConsoleKey.LeftArrow || keypress.Key == ConsoleKey.RightArrow)
            {
                StateHasChanged = true;
                confirm = !confirm;
                return PageEnum.SCRIPTS;
            }
            if(keypress.Key == ConsoleKey.Enter)
            {
                if(confirm){
                    StateHasChanged = true;
                    resultMarkup = RemoveScript(ListOfScripts[selection].Id);
                    ConfirmationBoxOpen = false;
                    ResultTextBoxOpen = true;
                    selection = 0;
                    confirm = false;
                    return PageEnum.SCRIPTS;
                }
                else
                {
                    StateHasChanged = true;
                    ConfirmationBoxOpen = false;
                    return PageEnum.SCRIPTS;
                }
            }
            StateHasChanged = false;
            return PageEnum.SCRIPTS;
        }


        if (UpdateTextBoxOpen)
        {
            if (!char.IsControl(keypress.KeyChar))
            {
                StateHasChanged = true;
                input.Append(keypress.KeyChar);
                return PageEnum.SCRIPTS;
            }else if(input.Length != 0 && keypress.Key == ConsoleKey.Backspace)
            {
                StateHasChanged = true;
                input.Remove(input.Length - 1, 1);
                return PageEnum.SCRIPTS;
            }
            else if(input.Length != 0 && keypress.Key == ConsoleKey.Enter)
            {
                switch (InputBoxStep)
                {
                    case 1:
                        StateHasChanged = true;
                        path = input.ToString();
                        input.Clear();
                        input.Append(ListOfScripts[selection].Name);
                        InputBoxStep++;
                        return PageEnum.SCRIPTS;
                    case 2:
                        StateHasChanged = true;
                        name = input.ToString();
                        input.Clear();
                        input.Append(ListOfScripts[selection].Description);
                        InputBoxStep++;
                        return PageEnum.SCRIPTS;
                    case 3:
                        StateHasChanged = true;
                        description = input.ToString();
                        input.Clear();
                        InputBoxStep = 1;
                        resultMarkup = UpdateScript(ListOfScripts[selection].Id);
                        UpdateTextBoxOpen = false;
                        ResultTextBoxOpen = true;
                        return PageEnum.SCRIPTS;
                    default:
                        throw new Exception("Not possible to get here");
                }
            }
        }


        if(keypress.KeyChar == '+')
        {
            StateHasChanged = true;
            AddTextBoxOpen = true;
            return PageEnum.SCRIPTS;
        }
        if(keypress.KeyChar == 'U' && ListOfScripts.Count > 0)
        {
            input.Append(ListOfScripts[selection].ScriptPath);
            StateHasChanged = true;
            UpdateTextBoxOpen = true;
            return PageEnum.SCRIPTS;
        }

        if(keypress.Key == ConsoleKey.Delete && ListOfScripts.Count > 0)
        {
            StateHasChanged = true;
            ConfirmationBoxOpen = true;
            return PageEnum.SCRIPTS;
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
        if (AddTextBoxOpen || ResultTextBoxOpen || ConfirmationBoxOpen || UpdateTextBoxOpen)
        {
            return SwapPanel();
        }
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
    }


    public Padder SwapPanel()
    {
        var middle = new Layout();
        var answer = confirm ? "> [bold]Yes[/]        No" : "  Yes      > [bold]No[/]";

        var inputLayout = new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Left(new Markup($"[blue]{input.ToString()}[slowblink]|[/][/]"), VerticalAlignment.Middle)
                            ).Expand().Header($"{InputBoxHeader[InputBoxStep]}").HeaderAlignment(Justify.Center).RoundedBorder().BorderColor(Color.Blue)
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

        if (AddTextBoxOpen || UpdateTextBoxOpen)
        {
            middle = inputLayout;
        }else if (ResultTextBoxOpen)
        {
            middle = resultLayout;
        }else if (ConfirmationBoxOpen)
        {
            middle = confirmationLayout;
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


    private Markup AddScript()
    {
        var resultMessage = scriptController.CreateScript(path, name, description);
        return resultMessage;
    }

    private Markup RemoveScript(int i)
    {
        var result = scriptController.DeleteScript(i);
        return result;
    }

    private Markup UpdateScript(int i)
    {
        var result = scriptController.UpdateScript(i, name, description, path);
        return result;
    }

    public override bool UpdateNeeded()
    {
        return StateHasChanged;
    }
}
