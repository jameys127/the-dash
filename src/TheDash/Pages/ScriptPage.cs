using Spectre.Console;

public class ScriptPage : BasePage
{
    bool AddTextBoxOpen = false;


    public override PageEnum PageNavigation(ConsoleKeyInfo keypress)
    {
        if(keypress.Key == ConsoleKey.Escape && AddTextBoxOpen)
        {
            AddTextBoxOpen = false;
            return PageEnum.SCRIPTS;
        }
        else if(keypress.Key == ConsoleKey.Escape)
        {
            return PageEnum.MENU;
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
        return new Padder(new Panel("stuff"), new Padding(10,2,10,2));
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
