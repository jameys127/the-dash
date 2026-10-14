using Spectre.Console;

public class ScriptPage : BasePage
{
    public override PageEnum PageNavigation(ConsoleKey key)
    {
        if(key == ConsoleKey.Escape)
        {
            return PageEnum.MENU;
        }
        return PageEnum.SCRIPTS;
    }

    public override Panel ReturnPanel()
    {
        var table = new Table();
        table.AddColumn("Scripts", col => col.Centered());
        table.AddColumn("Description", col => col.Centered());
        table.AddEmptyRow();
        table.AddEmptyRow();
        table.AddRow("Backup", "Back up computer and save it to my server");
        table.AddEmptyRow();
        // table.AddRow("Audio Fix", "Fixes the audio bug after some updates so that I can increase and decrease volume, not just bass");
        table.AddEmptyRow();
        Panel panel = new Panel(
            new Layout("root").SplitRows(
                new Layout("Title", new TitleRender("Scripts - Choose a script")).Size(1),
                new Layout("Body", new Padder(
                        Align.Center(
                            table.Border(TableBorder.SimpleHeavy)
                                .BorderColor(Color.Purple)
                                .Expand()
                        ), new Padding(10, 2, 10, 2)
                    )
                ),
                new Layout("Footer", new TitleRender("Press [ ESC ] to go back | Press [ + ] to add a script")).Size(1)
            )
        ).BorderColor(Color.Purple)
        .Padding(0,0,0,0)
        .NearBorder()
        .Expand();

        // Panel panel = new Panel(
        //     new Rows(
        //         new TitleRender("Scripts - Choose a script"),
        //         new Padder(Align.Center(
        //             table.Border(TableBorder.SimpleHeavy)
        //                 .BorderColor(Color.Purple)
        //                 .Expand()
        //         ), new Padding(10, 2, 10, 2)
        //         ),
        //         new TitleRender("Press [ ESC ] to go back | Press [ + ] to add a script")
        //     )
	    // )
        //     .BorderColor(Color.Purple)
	    //     .Padding(0, 0, 0, 0)
        //     .NearBorder()
        //     .Expand();
        return panel;
    }
}
