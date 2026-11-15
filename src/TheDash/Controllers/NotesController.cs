using Microsoft.Data.Sqlite;
using Spectre.Console;

public class NotesController{
    
    Database db;

    public NotesController(Database db){
        this.db = db;
    }

    public Markup CreateBook(string name){
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Books (name) VALUES ($name);
        """;
        command.Parameters.AddWithValue("$name", name);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]\u2713 Success:[/] Book created!");
        }catch (SqliteException){
            return new Markup("[red]\u2717 SQLite Error:[/] Couldn't create book! Please try again.");
        }
    }

    public Markup CreateNote(string name, string body, int bookId){
        using var connection = db.OpenScript();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Notes (name, body, book_id) VALUES ($name, $body, $book_id);
        """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$body", body);
        command.Parameters.AddWithValue("book_id", bookId);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]\u2713 Success:[/] Note created!");
        }catch (SqliteException){
            return new Markup("[red]\u2717 SQLite Error:[/] Couldn't create note! Please try again.");
        }
    }

    public List<Book> GetAllBooks(){
        List<Book> books = new List<Book>();
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, name FROM Books;
        """;
        using var reader = command.ExecuteReader();
        while (reader.Read()){
            books.Add(new Book(
                reader.GetInt32(0),
                reader.GetString(1)
            ));
        }
        return books;
    }

    public List<Note> GetAllNotesFromBook(int bookId){
        List<Note> notes = new List<Note>();
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, name, body, book_id FROM Notes WHERE book_id = $book_id;
        """;
        command.Parameters.AddWithValue("$book_id", bookId);
        using var reader = command.ExecuteReader();
        while (reader.Read()){
            notes.Add(new Note(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3)
            ));
        }
        return notes;
    }

    public Markup DeleteBook(int id){
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM Books WHERE id = $id;
        """;
        command.Parameters.AddWithValue("$id", id);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]✓ Success:[/] Book and its notes deleted!");
        }catch (SqliteException){
            return new Markup("[red]✗ SQLite Error:[/] Couldn't delete book! Please try again.");
        }
    }

    public Markup DeleteNote(int id){
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM Notes WHERE id = $id;
        """;
        command.Parameters.AddWithValue("$id", id);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]✓ Success:[/] Note deleted!");
        }catch (SqliteException){
            return new Markup("[red]✗ SQLite Error:[/] Couldn't delete note! Please try again.");
        }
    }

    public Markup UpdateBook(int id, string name){
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Books
            SET name = $name
            WHERE id = $id;
        """;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", name);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]✓ Success:[/] Book updated!");
        }catch (SqliteException){
            return new Markup("[red]✗ SQLite Error:[/] Couldn't update book! Please try again.");
        }
    }

    public Markup UpdateNote(int id, string name, string body, int bookId){
        using var connection = db.OpenNote();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Notes
            SET name = $name, body = $body, book_id = $book_id
            WHERE id = $id;
        """;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$body", body);
        command.Parameters.AddWithValue("$book_id", bookId);
        try{
            command.ExecuteNonQuery();
            return new Markup("[green]✓ Success:[/] Note updated!");
        }catch (SqliteException){
            return new Markup("[red]✗ SQLite Error:[/] Couldn't update note! Please try again.");
        }
    }
}
