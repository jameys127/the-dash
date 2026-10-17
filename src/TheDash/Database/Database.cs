using Microsoft.Data.Sqlite;

public class Database{

	private readonly string  _dbPath;

	public Database(string dbPath){
		_dbPath = dbPath;
	}

	public SqliteConnection Open(){
		var connection = new SqliteConnection($"Data Source={_dbPath}");
		connection.Open();
		return connection;
	}

	public void TablesIfNotExist(){
		using var connection = Open();
		var command = connection.CreateCommand();
		command.CommandText = """
			CREATE TABLE IF NOT EXISTS Scripts (
				id INTEGER PRIMARY KEY,
				name TEXT NOT NULL,
				description TEXT NOT NULL,
				path TEXT NOT NULL
			);
			CREATE TABLE IF NOT EXISTS Notes (
				id INTEGER PRIMARY KEY,
				name TEXT NOT NULL,
				body TEXT NOT NULL
			);
		""";
		command.ExecuteNonQuery();
	}
}
