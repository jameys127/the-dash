
public class Note{
    public int id {get;}
    public string name {get;}
    public string body {get;}
    public int book_id {get;}

    public Note(int id, string name, string body, int book_id){
        this.id = id;
        this.name = name;
        this.body = body;
        this.book_id = book_id;
    }
}
