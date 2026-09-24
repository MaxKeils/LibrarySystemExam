using System.Collections.ObjectModel;

namespace LibrarySystem;

public class Library
{
    private readonly object _ownerToken = new();
    private readonly List<Book> _books = new();
    private readonly List<Reader> _readers = new();
    private readonly Dictionary<Book, Reader> _loans = new();
    private readonly BorrowingPolicy _borrowingPolicy;

    public ReadOnlyCollection<Book> Books { get; }
    public ReadOnlyCollection<Reader> Readers { get; }
    public event EventHandler<LoanEventArgs>? BookBorrowed;
    public event EventHandler<LoanEventArgs>? BookReturned;

    public Library(BorrowingPolicy? borrowingPolicy = null)
    {
        _borrowingPolicy = borrowingPolicy ?? BorrowingPolicies.MaxConcurrentBooks(3);
        Books = _books.AsReadOnly();
        Readers = _readers.AsReadOnly();
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (book.OwnerToken is not null)
        {
            throw new InvalidOperationException("Книга уже добавлена в библиотеку");
        }

        _books.Add(book);
        book.OwnerToken = _ownerToken;
    }

    public void RegisterReader(Reader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        if (_readers.Any(existing => existing.Id == reader.Id))
        {
            throw new InvalidOperationException("Читатель с таким ID уже зарегистрирован");
        }

        _readers.Add(reader);
    }

    public bool BorrowBook(Reader reader, Book book)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(book);

        if (!_readers.Contains(reader) || !ReferenceEquals(book.OwnerToken, _ownerToken) || _loans.ContainsKey(book))
        {
            return false;
        }

        var activeLoanCount = _loans.Values.Count(borrower => ReferenceEquals(borrower, reader));
        if (!_borrowingPolicy(reader, book, activeLoanCount))
        {
            return false;
        }

        _loans.Add(book, reader);
        book.IsAvailable = false;
        BookBorrowed?.Invoke(this, new LoanEventArgs(book, reader));
        return true;
    }

    public bool ReturnBook(Reader reader, Book book)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(book);

        if (!_loans.TryGetValue(book, out var borrower) || !ReferenceEquals(borrower, reader))
        {
            return false;
        }

        _loans.Remove(book);
        book.IsAvailable = true;
        BookReturned?.Invoke(this, new LoanEventArgs(book, reader));
        return true;
    }

    public Reader? GetBorrower(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        return _loans.TryGetValue(book, out var reader) ? reader : null;
    }

    public Book? FindBook(string title)
    {
        foreach (var book in _books)
        {
            if (book.Title == title)
            {
                return book;
            }
        }

        return null;
    }

    public List<Book> GetAvailableBooks()
    {
        var availableBooks = new List<Book>();
        foreach (var book in _books)
        {
            if (!_loans.ContainsKey(book))
            {
                availableBooks.Add(book);
            }
        }

        return availableBooks;
    }
}
