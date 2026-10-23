using Spectre.Console;

public class NewListItem{
    private Database db;
    private ScriptController scriptController;

    public int InputStep {get; set;}
    private string PathToScript {get; set;}
    private string NameOfScript {get; set;}
    private string DescOfScript {get; set;}

    public NewListItem(Database db, ScriptController scriptController){
        this.db = db;
        this.scriptController = scriptController;
        InputStep = 1;
        PathToScript = "";
        NameOfScript = "";
        DescOfScript = "";
    }

    public Markup? AddToList(string input){
        switch(InputStep){
            case 1:
                PathToScript = input;
                InputStep++;
                return null;
            case 2:
                NameOfScript = input;
                InputStep ++;
                return null;
            case 3:
                DescOfScript = input;
                return AddToDb();
        }
        return null;
    }

    public Markup AddToDb(){
        Markup result = scriptController.CreateScript(PathToScript, NameOfScript, DescOfScript);
        return result;
    }
}

