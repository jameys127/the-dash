using Microsoft.Data.Sqlite;
using Spectre.Console;

public class ScriptController
{
    Database db;
    public ScriptController(Database db)
    {
        this.db = db;
    }

    public Markup CreateScript(string path, string name, string description)
    {
        using var connection = db.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            INSERT INTO Scripts (name, description, path) VALUES ('{name}', '{description}', '{path}');
        """;
        try
        {
            command.ExecuteNonQuery();
            return new Markup("[green]\u2713 Success:[/] Script added!");   
        }
        catch (SqliteException e)
        {
            return new Markup($"[red]\u2717 Error. Please try again:[/] {Markup.Escape(e.Message)}").Overflow(Overflow.Ellipsis);
        }
    }

    public List<Script> GetAllScripts()
    {
        List<Script> scripts = new List<Script>();
        using var connection = db.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT * FROM Scripts;
        """;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            scripts.Add(new Script(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            ));
        }
        return scripts;
    }
}
