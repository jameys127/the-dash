public class Script
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string ScriptPath {get; set;}
    public Script(int id, string name, string description, string path)
    {
        Id = id;
        Name = name;
        Description = description;
        ScriptPath = path;
    }
}