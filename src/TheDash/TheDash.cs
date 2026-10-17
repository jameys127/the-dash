using Microsoft.Data.Sqlite;

class Dash{
	static void Main()
	{

		var folder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"TheDash"
		);
		Directory.CreateDirectory(folder);
		var dbPath = Path.Combine(folder, "thedash.db");
		Database db = new Database(dbPath);
		db.TablesIfNotExist();

		MainPage mainPage = new MainPage(db);
		mainPage.StartDash();
	}
}	
