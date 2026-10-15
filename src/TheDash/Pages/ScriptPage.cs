using System.Text;
using Spectre.Console;

public class ScriptPage : BasePage
{
    bool AddTextBoxOpen = false;
    StringBuilder input = new StringBuilder();
    string path = "";
    string name = "";
    string description = "";


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
            return PageEnum.SCRIPTS;
        }


        if(keypress.Key == ConsoleKey.Add)
        {
            AddTextBoxOpen = true;       
        }
        return PageEnum.SCRIPTS;
    }



    public override Panel ReturnPanel()
    {
        Panel panel = new Panel(
            new Layout("root").SplitRows(
                new Layout("Title", new TitleRender("Scripts - Choose a script")).Size(1),
                new Layout("Body", MakeTable()),
                new Layout("Footer", new TitleRender("Press [ ESC ] to go back | Press [ + ] to add a script")).Size(1)
            )
        ).BorderColor(Color.Purple)
        .Padding(0,0,0,0)
        .NearBorder()
        .Expand();

        return panel;
    }


    public Padder SwapPanel()
    {
        return new Padder(new Layout("BodyRoot")
            .SplitRows(
                new Layout("UnusedTop").Ratio(3).Update(new Text("")),
                new Layout("MiddleRow").Size(5)
                    .SplitColumns(
                        new Layout("UnusedLeft").Ratio(1).Update(new Text("")),
                        new Layout("MiddleColumn").Ratio(2).MinimumSize(5).Update(
                            new Panel(
                                Align.Left(new Markup($"[blue]{input.ToString()}[slowblink]|[/][/]"), VerticalAlignment.Middle)
                            ).Expand().Header("[[ 1 / 3 ]] Enter the path to the script").RoundedBorder().BorderColor(Color.Blue)
                        ),
                        new Layout("UnusedRight").Ratio(1).Update(new Text(""))
                    ),
                new Layout("UnusedBottom").Ratio(3).Update(new Text(""))
            ), new Padding(0,0,0,0));

        // return new Padder(Align.Center(new Panel(
        //     new Rows(
        //         new Markup("[bold blue]Enter the path to the script: [/]")
        //     )
        // )
        // .RoundedBorder()
        // .BorderColor(Color.Blue), VerticalAlignment.Middle), new Padding(0,0,0,0));
    }

    public Padder MakeTable()
    {
        if (AddTextBoxOpen)
        {
            return SwapPanel();
        }
        var table = new Table();
        table.AddColumn("Scripts", col => col.Centered());
        table.AddColumn("Description", col => col.Centered());
        table.AddEmptyRow();
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        table.AddRow("Backup", "Back up everything on the computer except downloads and upload it to my server for storage");
        table.AddEmptyRow();
        table.AddRow("Audio Fix", "The script that fixes the issue of my volume button only changing bass and not actual volume. Run after every update");
        var padder = new Padder(
            Align.Center(
                table.Border(TableBorder.SimpleHeavy)
                    .BorderColor(Color.Purple)
                    .Expand()
            ), new Padding(10, 2, 10 ,2)
        );
        return padder;
    }
}
