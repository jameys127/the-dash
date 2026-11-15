using Microsoft.Data.Sqlite;
using Spectre.Console;

public class NotesController{
    
    Database db;

    public NotesController(Database db){
        this.db = db;
    }

    public void CreateBook(){}

    public void CreateNote(){}

    public void GetAllBooks(){}
    
    public void GetAllNotes(){}

    public void DeleteBook(){}

    public void DeleteNote(){}

    public void GetBook(){}

    public void GetNote(){}

    public void UpdateBook(){}

    public void UpdateNote(){}
}
