using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class LoanTests
{
    [TestMethod]
    public void BorrowBook_WhenAvailable_LibraryRecordsBorrower()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var book = NewBook("Первая");
        library.RegisterReader(reader);
        library.AddBook(book);

        Assert.IsTrue(library.BorrowBook(reader, book));
        Assert.AreSame(reader, library.GetBorrower(book));
        Assert.IsFalse(book.IsAvailable);
    }

    [TestMethod]
    public void BorrowBook_WhenUnavailable_DoesNotChangeBorrower()
    {
        var library = new Library();
        var firstReader = new Reader("Иван", 1);
        var secondReader = new Reader("Мария", 2);
        var book = NewBook("Первая");
        library.RegisterReader(firstReader);
        library.RegisterReader(secondReader);
        library.AddBook(book);
        library.BorrowBook(firstReader, book);

        Assert.IsFalse(library.BorrowBook(secondReader, book));
        Assert.AreSame(firstReader, library.GetBorrower(book));
    }

    [TestMethod]
    public void BorrowBook_AfterThreeBooks_RejectsFourthBook()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        library.RegisterReader(reader);
        var books = new[] { NewBook("Первая"), NewBook("Вторая"), NewBook("Третья") };
        foreach (var book in books)
        {
            library.AddBook(book);
            Assert.IsTrue(library.BorrowBook(reader, book));
        }

        var fourthBook = NewBook("Четвертая");
        library.AddBook(fourthBook);

        Assert.IsFalse(library.BorrowBook(reader, fourthBook));
        Assert.IsNull(library.GetBorrower(fourthBook));
        Assert.IsTrue(fourthBook.IsAvailable);
    }

    [TestMethod]
    public void ReturnBook_WhenBorrowed_LibraryClearsBorrower()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var book = NewBook("Первая");
        library.RegisterReader(reader);
        library.AddBook(book);
        library.BorrowBook(reader, book);

        Assert.IsTrue(library.ReturnBook(reader, book));
        Assert.IsNull(library.GetBorrower(book));
        Assert.IsTrue(book.IsAvailable);
    }

    [TestMethod]
    public void ReturnBook_FreesOneOfThreeBorrowingSlots()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        library.RegisterReader(reader);
        var books = new[] { NewBook("Первая"), NewBook("Вторая"), NewBook("Третья"), NewBook("Четвертая") };
        foreach (var book in books)
        {
            library.AddBook(book);
        }

        foreach (var book in books.Take(3))
        {
            Assert.IsTrue(library.BorrowBook(reader, book));
        }

        Assert.IsTrue(library.ReturnBook(reader, books[0]));
        Assert.IsTrue(library.BorrowBook(reader, books[3]));
        Assert.AreSame(reader, library.GetBorrower(books[3]));
    }

    [TestMethod]
    public void ReturnBook_ByAnotherReader_DoesNotChangeBorrower()
    {
        var library = new Library();
        var owner = new Reader("Иван", 1);
        var otherReader = new Reader("Мария", 2);
        var book = NewBook("Первая");
        library.RegisterReader(owner);
        library.RegisterReader(otherReader);
        library.AddBook(book);
        library.BorrowBook(owner, book);

        Assert.IsFalse(library.ReturnBook(otherReader, book));
        Assert.AreSame(owner, library.GetBorrower(book));
        Assert.IsFalse(book.IsAvailable);
    }

    private static Book NewBook(string title) => new(title, "Автор", 2020);
}
