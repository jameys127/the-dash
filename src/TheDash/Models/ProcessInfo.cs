using System;

public class ProcessInfo{
    public bool IsWindows {get;}
    public Script ScriptInfo {get;}

    public ProcessInfo(Script script){
        ScriptInfo = script;
        IsWindows = OperatingSystem.IsWindows();
    }


    public ScriptFileInfo ParseScript(){
        string fileExtension = Path.GetExtension(ScriptInfo.ScriptPath);
        string fileName = Path.GetFileName(ScriptInfo.ScriptPath);
        string? filePath = Path.GetDirectoryName(ScriptInfo.ScriptPath);
        if(filePath != null){
            return new ScriptFileInfo(fileExtension, fileName, filePath);
        }else{
            throw new Exception("Path directory name error");
        }
    }
}

