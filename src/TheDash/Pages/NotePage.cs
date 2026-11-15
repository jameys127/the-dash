using Spectre.Console;
using Spectre.Console.Rendering;

public class NotePage : BasePage{
    
    private Database db;
    private NotesController notesController;

    private int selection;
    private bool booksSelected;

    

    public NotePage(Database db){
        this.db = db;
        notesController = new NotesController(db);
        selection = 0;
        booksSelected = true;
    }


    public override PageEnum PageNavigation(ConsoleKeyInfo keypress){
        switch(keypress){
            case {Key : ConsoleKey.UpArrow}:
                return PageEnum.NOTES;
            case {Key : ConsoleKey.DownArrow}:
                return PageEnum.NOTES;
            case {Key : ConsoleKey.Escape}:
                StateHasChanged = true;
                return PageEnum.MENU;
        }
        return PageEnum.NOTES;
    }

    public override IRenderable ReturnRender(){
        Layout panel = new Layout("root").SplitRows(
            new Layout("Title", new TitleRender("Notes - Choose note or create a new one")).Size(1),
            new Layout("Body", MakeLayout()),
            new Layout("Footer", new TitleRender("Press [ ESC ] to go back | Press [ ? ] for help")).Size(1)
        );
        return panel;
    }

    public override bool UpdateNeeded(){
        return StateHasChanged;
    }

    public Layout MakeLayout(){
        return new Layout("BodyRoot").SplitColumns(
            new Layout("LeftBooks").Ratio(1).Update(new Panel("").Expand().RoundedBorder().BorderColor(Color.Purple)),
            new Layout("MiddleNotes").Ratio(1).Update(new Panel("").Expand().RoundedBorder().BorderColor(Color.Purple)),
            new Layout("RightNoteBody").Ratio(2).Update(new Panel("").Expand().RoundedBorder().BorderColor(Color.Purple))
        );
    }
}
