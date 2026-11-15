using Microsoft.Data.Sqlite;

class Dash{
	static void Main()
	{

		var folder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"TheDash"
		);
		Directory.CreateDirectory(folder);
		var dbPathScript = Path.Combine(folder, "thedashscripts.db");
		var dbPathNote = Path.Combine(folder, "thedashnotes.db");
		Database db = new Database(dbPathScript, dbPathNote);
		db.TablesIfNotExist();

		MainPage mainPage = new MainPage(db);
		mainPage.StartDash();
	}
}	
