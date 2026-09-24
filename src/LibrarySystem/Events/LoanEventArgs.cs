namespace LibrarySystem;

public sealed class LoanEventArgs : EventArgs
{
    public Book Book { get; }
    public Reader Reader { get; }

    public LoanEventArgs(Book book, Reader reader)
    {
        Book = book;
        Reader = reader;
    }
}
