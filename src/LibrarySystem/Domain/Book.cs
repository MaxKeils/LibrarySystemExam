namespace LibrarySystem;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsAvailable => OwningLibrary?.GetBorrower(this) is null;

    internal Library? OwningLibrary { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }
}
