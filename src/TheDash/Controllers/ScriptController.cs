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
        command.CommandText = """
            INSERT INTO Scripts (name, description, path) VALUES ($name, $description, $path);
        """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$description", description);
        command.Parameters.AddWithValue("$path", path);
        try
        {
            command.ExecuteNonQuery();
            return new Markup("[green]\u2713 Success:[/] Script added!");   
        }
        catch (SqliteException)
        {
            return new Markup($"[red]\u2717 SQLite Error: [/]Couldn't save script! Please try again.");
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

    public Markup DeleteScript(int i)
    {
        using var connection = db.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM Scripts WHERE id = $i;
        """;
        command.Parameters.AddWithValue("$i", i);
        try
        {
            command.ExecuteNonQuery();
            return new Markup($"[green]\u2713 Success:[/] Script deleted!");
        }
        catch (SqliteException)
        {
            return new Markup($"[red]\u2717 SQLite Error: [/]Couldn't delete script! Please try again.");
        }
    }

    public Markup UpdateScript(int id, string name, string description, string path)
    {
        using var connection = db.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Scripts
            SET name = $name, description = $description, path = $path
            WHERE id = $id;
        """;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$description", description);
        command.Parameters.AddWithValue("$path", path);
        try
        {
            command.ExecuteNonQuery();
            return new Markup($"[green]\u2713 Success:[/] Script updated!");
        }
        catch (SqliteException)
        {
            return new Markup($"[red]\u2717 SQLite Error: [/]Couldn't update script! Please try again.");
        }
    }
}
