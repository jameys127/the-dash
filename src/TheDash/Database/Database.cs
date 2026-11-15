using Microsoft.Data.Sqlite;

public class Database{

	private readonly string _dbPathScript;
    private readonly string _dbPathNote;

	public Database(string dbPathScript, string dbPathNote){
		_dbPathScript = dbPathScript;
        _dbPathNote = dbPathNote;
	}

	public SqliteConnection OpenScript(){
		var connection = new SqliteConnection($"Data Source={_dbPathScript};Foreign Keys=True");
		connection.Open();
		return connection;
	}
    public SqliteConnection OpenNote(){
        var connection = new SqliteConnection($"Data Source={_dbPathNote};Foreign Keys=True");
        connection.Open();
        return connection;
    }

	public void TablesIfNotExist(){
		using var connectionScript = OpenScript();
		var commandScript = connectionScript.CreateCommand();
		commandScript.CommandText = """
            CREATE TABLE IF NOT EXISTS Scripts (
                id INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                description TEXT NOT NULL,
                path TEXT NOT NULL
            );
        """;

        using var connectionNote = OpenNote();
        var commandNote = connectionNote.CreateCommand();
        commandNote.CommandText = """
            CREATE TABLE IF NOT EXISTS Books (
                id INTEGER PRIMARY KEY,
                name TEXT NOT NULL UNIQUE
            );
            CREATE TABLE IF NOT EXISTS Notes (
                id INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                body TEXT NOT NULL,
                book_id INTEGER NOT NULL,
                FOREIGN KEY (book_id) REFERENCES Books(id) ON DELETE CASCADE,
                UNIQUE (book_id, name)
            );
        """;

        commandNote.ExecuteNonQuery();    
		commandScript.ExecuteNonQuery();
	}
}
