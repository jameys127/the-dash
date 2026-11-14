

public class ScriptFileInfo{
    public TypeOfScript ScriptType {get; set;}
    public string ScriptFile {get; set;}
    public string ScriptPath {get; set;}

    public ScriptFileInfo(string ScriptType, string ScriptFile, string ScriptPath){
        this.ScriptType = FindScriptType(ScriptType);
        this.ScriptFile = ScriptFile;
        this.ScriptPath = ScriptPath;
    }

    public TypeOfScript FindScriptType(string input){
        input = input.ToLowerInvariant();
        switch(input){
            case ".sh":
                return TypeOfScript.BASH;
            case ".bat":
                return TypeOfScript.BATCH;
            case ".cmd":
                return TypeOfScript.BATCH;
            case ".ps1":
                return TypeOfScript.POWERSHELL;
            case ".py":
                return TypeOfScript.PYTHON;
            case ".exe":
                return TypeOfScript.EXE;
            default:
                throw new Exception("Not a supported file type");
        }
    }

    
}


public enum TypeOfScript{
    BASH,
    BATCH,
    POWERSHELL,
    PYTHON,
    EXE
}
