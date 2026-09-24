using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class LibraryTests
{
    [TestMethod]
    public void AddBook_AddsBookToCollection()
    {
        var library = new Library();
        var book = NewBook("Первая");

        library.AddBook(book);

        CollectionAssert.AreEqual(new[] { book }, library.Books);
    }

    [TestMethod]
    public void RegisterReader_AddsReaderToCollection()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);

        library.RegisterReader(reader);

        CollectionAssert.AreEqual(new[] { reader }, library.Readers);
    }

    [TestMethod]
    public void FindBook_ReturnsBookWithMatchingTitle()
    {
        var library = new Library();
        var firstBook = NewBook("Первая");
        var matchingBook = NewBook("Искомая");
        library.AddBook(firstBook);
        library.AddBook(matchingBook);

        var result = library.FindBook("Искомая");

        Assert.AreSame(matchingBook, result);
    }

    [TestMethod]
    public void FindBook_WhenTitleIsMissing_ReturnsNull()
    {
        var library = new Library();
        library.AddBook(NewBook("Первая"));

        var result = library.FindBook("Неизвестная");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetAvailableBooks_ReturnsOnlyBooksThatCanBeBorrowed()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var availableBook = NewBook("Доступная");
        var borrowedBook = NewBook("Выданная");
        library.RegisterReader(reader);
        library.AddBook(availableBook);
        library.AddBook(borrowedBook);
        library.BorrowBook(reader, borrowedBook);

        var result = library.GetAvailableBooks();

        CollectionAssert.AreEqual(new[] { availableBook }, result);
    }

    [TestMethod]
    public void GetAvailableBooks_WhenNoBooksAreAvailable_ReturnsEmptyList()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var book = NewBook("Выданная");
        library.RegisterReader(reader);
        library.AddBook(book);
        library.BorrowBook(reader, book);

        var result = library.GetAvailableBooks();

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void BorrowBook_RequiresRegisteredReaderAndBookFromLibrary()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var book = NewBook("В фонде");
        var otherBook = NewBook("Не в фонде");
        library.AddBook(book);

        Assert.IsFalse(library.BorrowBook(reader, book));
        library.RegisterReader(reader);
        Assert.IsFalse(library.BorrowBook(reader, otherBook));
        Assert.IsNull(library.GetBorrower(book));
        Assert.IsNull(library.GetBorrower(otherBook));
    }

    private static Book NewBook(string title) => new(title, "Автор", 2020);
}
