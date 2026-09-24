using System.Collections.ObjectModel;

namespace LibrarySystem;

public class Library
{
    private const int BorrowLimit = 3;

    private readonly List<Book> _books = new();
    private readonly List<Reader> _readers = new();
    private readonly Dictionary<Book, Reader> _loans = new();

    public ReadOnlyCollection<Book> Books { get; }
    public ReadOnlyCollection<Reader> Readers { get; }

    public Library()
    {
        Books = _books.AsReadOnly();
        Readers = _readers.AsReadOnly();
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (book.OwningLibrary is not null)
        {
            throw new InvalidOperationException("Книга уже добавлена в библиотеку");
        }

        _books.Add(book);
        book.OwningLibrary = this;
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
        if (!_readers.Contains(reader) || !ReferenceEquals(book.OwningLibrary, this) || _loans.ContainsKey(book))
        {
            return false;
        }

        if (_loans.Values.Count(borrower => ReferenceEquals(borrower, reader)) >= BorrowLimit)
        {
            return false;
        }

        _loans.Add(book, reader);
        return true;
    }

    public bool ReturnBook(Reader reader, Book book)
    {
        if (!_loans.TryGetValue(book, out var borrower) || !ReferenceEquals(borrower, reader))
        {
            return false;
        }

        return _loans.Remove(book);
    }

    public Reader? GetBorrower(Book book)
    {
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
