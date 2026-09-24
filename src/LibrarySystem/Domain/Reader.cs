namespace LibrarySystem;

public class Reader
{
    public string Name { get; }
    public int Id { get; }

    public Reader(string name, int id)
    {
        Name = name;
        Id = id;
    }
}
