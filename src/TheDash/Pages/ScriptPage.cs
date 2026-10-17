using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

public class ScriptPage : BasePage
{
    private Database db;
    private ScriptController scriptController;
    private bool AddTextBoxOpen = false;
    private bool ResultTextBoxOpen = false;
    private int InputBoxStep = 1;
    private Dictionary<int, string> InputBoxHeader = new Dictionary<int, string> {
        {1, "[[ 1 / 3 ]] Enter the [bold italic]path[/] to the script"},
        {2, "[[ 2 / 3 ]] Enter the [bold italic]name[/] of the script"},
        {3, "[[ 3 / 3 ]] Enter the [bold italic]description[/] of the script"}
    };
    private StringBuilder input = new StringBuilder();
    private string path = "";
    private string name = "";
    private string description = "";


    public ScriptPage(Database db)
    {
        this.db = db;
        scriptController = new ScriptController(db);
    }


    public override PageEnum PageNavigation(ConsoleKeyInfo keypress)
    {
        if(keypress.Key == ConsoleKey.Escape && AddTextBoxOpen)
        {
            AddTextBoxOpen = false;
            input.Clear();
            return PageEnum.SCRIPTS;
        }
        else if(keypress.Key == ConsoleKey.Escape)
        {
            return PageEnum.MENU;
        }

        if (AddTextBoxOpen)
        {
            if (!char.IsControl(keypress.KeyChar))
            {
                input.Append(keypress.KeyChar);
            }else if(input.Length != 0 && keypress.Key == ConsoleKey.Backspace)
            {
                input.Remove(input.Length - 1, 1);
            }
            else if(input.Length != 0 && keypress.Key == ConsoleKey.Enter)
            {
                switch (InputBoxStep)
                {
                    case 1:
                        path = input.ToString();
                        input.Clear();
                        InputBoxStep++;
                        break;
                    case 2:
                        name = input.ToString();
                        input.Clear();
                        InputBoxStep++;
                        break;
                    case 3:
                        description = input.ToString();
                        input.Clear();
                        InputBoxStep = 1;
                        var resultMarkup = AddScript();
                        AddTextBoxOpen = false;
                        ResultTextBoxOpen = true;
                        break;
                    default:
                        throw new Exception("Not possible to get here");
                }
            }
            return PageEnum.SCRIPTS;
        }

        if (ResultTextBoxOpen)
        {
            //TODO Make the success or failure box. Another method.
        }


        if(keypress.Key == ConsoleKey.Add)
        {
            AddTextBoxOpen = true;       
        }
        return PageEnum.SCRIPTS;
    }



    public override IRenderable ReturnRender()
    {
        // Panel panel = new Panel(
            Layout panel = new Layout("root").SplitRows(
                new Layout("Title", new TitleRender("Scripts - Choose a script")).Size(1),
                new Layout("Body", MakeTable()),
                new Layout("Footer", new TitleRender("Press [ ESC ] to go back | Press [ + ] to add a script")).Size(1)
            );
        // ).BorderColor(Color.Purple)
        // .Padding(0,0,0,0)
        // .NearBorder()
        // .Expand();

        return panel;
    }


    public Padder SwapPanel()
    {
        return new Padder(new Layout("BodyRoot")
            .SplitRows(
                new Layout("UnusedTop").Ratio(3).Update(new Text("")),
                // ^^ Simply blank space ^^

                new Layout("MiddleRow").Size(5)
                    .SplitColumns(
                        new Layout("UnusedLeft").Ratio(1).Update(new Text("")),
                        // ^^ Simply blank space ^^

                        new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Left(new Markup($"[blue]{input.ToString()}[slowblink]|[/][/]"), VerticalAlignment.Middle)
                            ).Expand().Header($"{InputBoxHeader[InputBoxStep]}").RoundedBorder().BorderColor(Color.Blue)
                        ),

                        // vv Simply blank space vv
                        new Layout("UnusedRight").Ratio(1).Update(new Text(""))
                    ),

                // vv Simply blank space vv
                new Layout("UnusedBottom").Ratio(3).Update(new Text(""))
            ), new Padding(0,0,0,0));
    }

    public Padder MakeTable()
    {
        if (AddTextBoxOpen)
        {
            return SwapPanel();
        }
        var table = new Table();
        table.AddColumn("[purple]Scripts[/]", col => col.Centered());
        table.AddColumn("[purple]Description[/]", col => col.Centered());
        table.AddEmptyRow();
        var padder = new Padder(
            Align.Center(
                table.Border(TableBorder.SimpleHeavy)
                    .BorderColor(Color.Purple)
                    .Expand()
            ), new Padding(10, 2, 10 ,2)
        );
        return padder;
    }

    private Markup AddScript()
    {
        var resultMessage = scriptController.CreateScript(path, name, description);
        return resultMessage;
    }
}
